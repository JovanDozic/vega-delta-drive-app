import { Vehicle } from './vehicle.model';

export interface VehicleSearchResponse extends Vehicle {
  distanceFromPassenger: number;
  estimatedPrice: number;
  didDecline: boolean;
  isLoadingRequest: boolean;
}
