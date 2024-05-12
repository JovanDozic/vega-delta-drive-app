import { Location } from './location.model';
import { User } from './user.model';
import { Vehicle } from './vehicle.model';

export interface VehicleBooking {
  id: number;
  userId: number;
  user: User;
  vehicleId: number;
  vehicle: Vehicle;
  bookingDate: Date;
  startLocation: Location;
  endLocation: Location;
  price: number;
  rating: number;
}
