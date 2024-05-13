import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import {
  HTTP_INTERCEPTORS,
  HttpClientModule,
  provideHttpClient,
  withFetch,
} from '@angular/common/http';
import { LoginComponent } from './authentication/login/login.component';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from './common/navbar/navbar.component';
import { VehicleBookingComponent } from './vehicle-booking/vehicle-booking.component';
import { AuthInterceptor } from './auth.interceptor';
import { VehicleBookingTrackingComponent } from './vehicle-booking-tracking/vehicle-booking-tracking.component';
import { VehicleBookingHistoryComponent } from './vehicle-booking-history/vehicle-booking-history.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegistrationComponent,
    NavbarComponent,
    VehicleBookingComponent,
    VehicleBookingTrackingComponent,
    VehicleBookingHistoryComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, FormsModule, HttpClientModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
