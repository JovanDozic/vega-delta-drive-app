namespace DeltaDrive.DA.Contracts.Shared
{
    public enum VehicleBookingStatus
    {
        Waiting = 0,
        DrivingToStartLocation = 1,
        WaitingForPassenger = 2,
        DrivingToEndLocation = 3,
        Completed = 4,
    }
}
