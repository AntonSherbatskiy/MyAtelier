using System.Runtime.InteropServices.JavaScript;
using ErrorOr;
using Application.Models;
using MyAtelier.DAL.Entities;

namespace Application.ErrorModels;

public static class Errors
{

    public static class Authentication
    {
        public static Error UserAlreadyExists => Error.Conflict(description: "User already exists");
        public static Error IncorrectPassword => Error.Conflict(description: "Password is incorrect");
        public static Error PasswordMismatch => Error.Unauthorized(description: "Passwords do not match");
        public static Error UserDoesNotExist => Error.Unauthorized(description: "User with given email does not exist");
        public static Error IncorrectConfirmationCode => Error.Failure(description: "Incorrect confirmation code");
    }
    
    public static class Clothing
    {
        public static Error ClothingAlreadyExists => Error.Conflict(description: "Clothing already exists");
        public static Error IncorrectClothingSize =>
            Error.Failure(description: "Incorrect clothing size. Correct are: 'S', 'M', 'L', 'XL'");
        public static Error IncorrectClothing =>
            Error.Failure(description: "Clothing does not exist. Add clothing first");

        public static Error CannotRemoveActiveClothing =>
            Error.Failure(description: "Can't remove clothes as some services use it");
    }

    public static class Material
    {
        public static Error MaterialAlreadyExists => Error.Conflict(description: "Material already exists");
        public static Error IncorrectQuantity => Error.Failure(description: "Incorrect material quantity");

        public static Error CannotRemoveActiveMaterial =>
            Error.Failure(description: "Cannot remove material because some services use it");

        public static Error IncorrectMaterial = Error.Failure(description: "Material for this service does not exist");
    }

    public static class Service
    {
        public static Error ServiceAlreadyExists =>
            Error.Conflict(description: "This service is already exists");

        public static Error IncorrectService =>
            Error.NotFound(description: "Service with this id does not exist");

        public static Error CannotRemoveActiveService =>
            Error.Failure(description: "Cannot remove active service. Some active orders use this service");

        public static Error IncorrectPrice =>
            Error.Failure(description: "Incorrect price");

        public static Error IncorrectEstimatedDays =>
            Error.Failure(description: "Incorrect estimated days");
    }

    public static class Order
    {
        public static Error IncorrectServiceCategory
            => Error.Failure(
                description:
                "Service with this category does not exist. Available categories are: 'Sewing', 'Repairing'");

        public static Error IncorrectService
            => Error.Failure(description: "Service with given id does not exist in this given category");

        public static Error OrderIsAlreadyPlaced
            => Error.Failure(description: "This order is already placed");

        public static Error IncorrectOrder =>
            Error.Failure(description: "Order does not exists");

        public static Error OrderFinished =>
            Error.Failure(description: "This order is already completed or canceled");

        public static Error IncorrectStatus =>
            Error.Failure(description: "Incorrect order status. Possible statuses are: 'Completed', 'Canceled'");

        public static Error CannotRemoveProcessOrder =>
            Error.Failure(description: "It is not allowed to remove order with status 'Process'");

        public static Error CannotCancelFinishedOrder =>
            Error.Failure(description: "Cannot cancel finished order");

        public static Error CannotCompleteFinishedOrder =>
            Error.Failure(description: "Cannot complete finished order");

        public static Error NotEnoughMaterial =>
            Error.Failure(description: "This service is temporary unavailable. Not enough material to place sewing order");
    }

    public static class User
    {
        public static Error SomeOrdersAreProcess =>
            Error.Failure(description: "Cannot remove account because some orders are in process");
    }

    public static class Role
    {
        public static Error InvalidRole =>
            Error.Failure(description: "Role does not exists");
    }
}