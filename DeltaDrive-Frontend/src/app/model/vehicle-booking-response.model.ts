import { VehicleBookingRequest } from './vehicle-booking-request.model';

export interface VehicleBookingResponse extends VehicleBookingRequest {
  isAccepted: boolean;
  bookingId?: number;
}
