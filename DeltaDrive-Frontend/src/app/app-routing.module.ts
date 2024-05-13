import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './authentication/login/login.component';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { VehicleBookingComponent } from './vehicle-booking/vehicle-booking.component';
import { authGuard } from './auth.guard';
import { VehicleBookingTrackingComponent } from './vehicle-booking-tracking/vehicle-booking-tracking.component';
import { VehicleBookingHistoryComponent } from './vehicle-booking-history/vehicle-booking-history.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },

  { path: 'home', component: HomeComponent },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegistrationComponent },

  {
    path: 'vehicle-booking',
    component: VehicleBookingComponent,
    canActivate: [authGuard],
  },

  {
    path: 'vehicle-booking-tracking/:id',
    component: VehicleBookingTrackingComponent,
    canActivate: [authGuard],
  },

  {
    path: 'vehicle-booking-history',
    component: VehicleBookingHistoryComponent,
    canActivate: [authGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
