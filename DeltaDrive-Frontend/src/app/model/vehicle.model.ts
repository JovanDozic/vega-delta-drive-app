import { Location } from './location.model';
import { Point } from './point.model';

export interface Vehicle {
  id: number;
  brand: string;
  firstName: string;
  lastName: string;
  location: Point;
  startPrice: number;
  pricePerKm: number;
  isBooked: boolean;
  rating: number;
}
