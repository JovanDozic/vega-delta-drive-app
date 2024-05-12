import { Location } from './location.model';

export interface VehicleSearchRequest {
  startLocation: Location;
  endLocation: Location;
}
