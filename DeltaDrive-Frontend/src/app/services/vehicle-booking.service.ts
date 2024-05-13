import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VehicleBookingRequest } from '../model/vehicle-booking-request.model';
import { environment } from '../../environments/environment';
import {
  VehicleBooking,
  VehicleBookingStatus,
} from '../model/vehicle-booking.model';
import { VehicleBookingResponse } from '../model/vehicle-booking-response.model';
import { Result } from '../model/common/result.model';

@Injectable({
  providedIn: 'root',
})
export class VehicleBookingService {
  constructor(private http: HttpClient) {}

  sendBookingRequest(request: VehicleBookingRequest) {
    return this.http.post<Result<VehicleBookingResponse>>(
      `${environment.apiHost}/VehicleBooking/sendRequest`,
      request
    );
  }

  getBooking(id: number) {
    return this.http.get<Result<VehicleBooking>>(
      `${environment.apiHost}/VehicleBooking/getBooking/${id}`
    );
  }

  startRideToStartLocation(id: number) {
    return this.http.get<any>(
      `${environment.apiHost}/VehicleBooking/startRideToStartLocation/${id}`,
      {}
    );
  }

  startRideToEndLocation(id: number) {
    return this.http.get<any>(
      `${environment.apiHost}/VehicleBooking/startRideToEndLocation/${id}`,
      {}
    );
  }

  completeBooking(booking: VehicleBooking) {
    return this.http.patch<Result<VehicleBooking>>(
      `${environment.apiHost}/VehicleBooking/completeBooking`,
      booking
    );
  }

  getHistory() {
    return this.http.get<VehicleBooking[]>(
      `${environment.apiHost}/VehicleBooking/history`
    );
  }

  updateBooking(booking: VehicleBooking) {
    return this.http.patch<any>(
      `${environment.apiHost}/VehicleBooking/rate`,
      booking
    );
  }

  getStatusAsString(status: VehicleBookingStatus): string {
    switch (status) {
      case VehicleBookingStatus.Waiting:
        return 'Waiting';
      case VehicleBookingStatus.DrivingToStartLocation:
        return 'Driving to Start Location';
      case VehicleBookingStatus.WaitingForPassenger:
        return 'Waiting for Passenger';
      case VehicleBookingStatus.DrivingToEndLocation:
        return 'Driving to End Location';
      case VehicleBookingStatus.Completed:
        return 'Completed';
      default:
        return 'Unknown Status';
    }
  }
}
