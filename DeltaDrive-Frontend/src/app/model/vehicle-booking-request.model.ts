import { Location } from './location.model';

export interface VehicleBookingRequest {
  id: number;
  userId: number;
  vehicleId: number;
  startLocation: Location;
  endLocation: Location;
}
