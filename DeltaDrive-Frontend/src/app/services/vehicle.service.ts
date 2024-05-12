import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VehicleSearchRequest } from '../model/vehicle-search-request.model';
import { environment } from '../../environments/environment';
import { PagedResult } from '../model/common/paged-result.model';
import { Vehicle } from '../model/vehicle.model';
import { VehicleSearchResponse } from '../model/vehicle-search-response.model';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  constructor(private http: HttpClient) {}

  getAvailableVehicles(search: VehicleSearchRequest) {
    return this.http.post<PagedResult<VehicleSearchResponse>>(
      `${environment.apiHost}/Vehicle/getAvailable`,
      search
    );
  }
}
