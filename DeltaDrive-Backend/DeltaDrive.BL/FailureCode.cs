using FluentResults;

namespace DeltaDrive.BL
{
    public static class FailureCode
    {
        public static readonly IError NonUniqueEmail = new Error("User with supplied email already exists.")
            .WithMetadata("code", 400)
            .WithMetadata("subCode", 1);
        public static readonly IError UserNotFound = new Error("Account with supplied email is not found.").WithMetadata("code", 404)
            .WithMetadata("subCode", 2);
        public static readonly IError InvalidCredentials = new Error("Invalid email or password.").WithMetadata("code", 404)
            .WithMetadata("subCode", 2);

        public static readonly IError InvalidArgument = new Error("Invalid data supplied.")
            .WithMetadata("code", 400);
        public static readonly IError Forbidden = new Error("Access to resource is restricted.")
            .WithMetadata("code", 403);
        public static readonly IError NotFound = new Error("Accessed resource not found.")
            .WithMetadata("code", 404);
        public static readonly IError Conflict = new Error("Database persistence conflict exception.")
            .WithMetadata("code", 409);
        public static readonly IError Internal = new Error("Internal server error.")
            .WithMetadata("code", 500);

        public static readonly IError UserAlreadyHasAnActiveBooking = new Error("User already has an active booking.")
            .WithMetadata("code", 400)
            .WithMetadata("subCode", 3);

    }
}
