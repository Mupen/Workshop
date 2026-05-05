using ShoppingCart.Domain.Contracts;
using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Domain.Entities;

public sealed class User
{
    /// <summary>
    /// Empty constructor used by Entity Framework when it rebuilds this object from the database.
    /// </summary>
    private User()
    {
        Email = string.Empty;
        DisplayName = string.Empty;
        PasswordHash = string.Empty;
    }

    /// <summary>
    /// Creates a user object after the public factory method has checked the rules.
    /// </summary>
    private User(
        Guid id,
        string email,
        string displayName,
        string passwordHash,
        UserRole role,
        DateTime createdAtUtc)
    {
        Id = id;
        Email = NormalizeEmail(email);
        DisplayName = displayName.Trim();
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string DisplayName { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Creates a new user account if the account information is valid.
    /// </summary>
    public static Result<User> Create(
        string email,
        string displayName,
        string passwordHash,
        UserRole role,
        DateTime? createdAtUtc = null,
        Guid? id = null)
    {
        var result = ValidateEmail(email);
        if (result.IsFailure)
            return result.ToFailure<User>();

        result = ValidateDisplayName(displayName);
        if (result.IsFailure)
            return result.ToFailure<User>();

        result = ValidatePasswordHash(passwordHash);
        if (result.IsFailure)
            return result.ToFailure<User>();

        result = ValidateRole(role);
        if (result.IsFailure)
            return result.ToFailure<User>();

        var user = new User(
            id ?? Guid.NewGuid(),
            email,
            displayName,
            passwordHash,
            role,
            createdAtUtc ?? DateTime.UtcNow);

        return Result<User>.Success(user);
    }

    /// <summary>
    /// Changes the name shown for this user account.
    /// </summary>
    public Result ChangeDisplayName(string displayName)
    {
        var result = ValidateDisplayName(displayName);
        if (result.IsFailure)
            return result;

        DisplayName = displayName.Trim();
        return Result.Success();
    }

    /// <summary>
    /// Changes the user's role, which controls what the user is allowed to do.
    /// </summary>
    public Result ChangeRole(UserRole role)
    {
        var result = ValidateRole(role);
        if (result.IsFailure)
            return result;

        Role = role;
        return Result.Success();
    }

    /// <summary>
    /// Replaces the stored password hash after a new password has been hashed.
    /// </summary>
    public Result ChangePasswordHash(string passwordHash)
    {
        var result = ValidatePasswordHash(passwordHash);
        if (result.IsFailure)
            return result;

        PasswordHash = passwordHash;
        return Result.Success();
    }

    /// <summary>
    /// Marks the user as active so the account can be used again.
    /// </summary>
    public Result Activate()
    {
        IsActive = true;
        return Result.Success();
    }

    /// <summary>
    /// Marks the user as inactive so the account cannot be used.
    /// </summary>
    public Result Deactivate()
    {
        IsActive = false;
        return Result.Success();
    }

    /// <summary>
    /// Checks that the email looks like a basic email address and is not too long.
    /// </summary>
    private static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure(
                new Error("User.InvalidEmail", "User email is required."));
        }

        var normalizedEmail = NormalizeEmail(email);
        if (normalizedEmail.Length > 254)
        {
            return Result.Failure(
                new Error("User.EmailTooLong", "User email cannot be longer than 254 characters."));
        }

        if (!normalizedEmail.Contains('@') || normalizedEmail.StartsWith('@') || normalizedEmail.EndsWith('@'))
        {
            return Result.Failure(
                new Error("User.InvalidEmail", "User email must be a valid email address."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the display name is not empty and is not too long.
    /// </summary>
    private static Result ValidateDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            return Result.Failure(
                new Error("User.InvalidDisplayName", "User display name is required."));
        }

        if (displayName.Trim().Length > 100)
        {
            return Result.Failure(
                new Error("User.DisplayNameTooLong", "User display name cannot be longer than 100 characters."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that a password hash exists. The plain password is never stored here.
    /// </summary>
    private static Result ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure(
                new Error("User.InvalidPasswordHash", "User password hash is required."));
        }

        return Result.Success();
    }

    /// <summary>
    /// Checks that the user role is one of the known account roles.
    /// </summary>
    private static Result ValidateRole(UserRole role)
    {
        if (!Enum.IsDefined(role))
        {
            return Result.Failure(
                new Error("User.InvalidRole", "User role is not valid."));
        }

        return Result.Success();
    }

    private static string NormalizeEmail(string email)
        => email.Trim().ToLowerInvariant();
}
