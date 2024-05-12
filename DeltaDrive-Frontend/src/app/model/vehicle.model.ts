import { Location } from './location.model';

export interface Vehicle {
  id: number;
  brand: string;
  firstName: string;
  lastName: string;
  location: Location;
  startPrice: number;
  pricePerKm: number;
  isBooked: boolean;
  rating: number;
}
