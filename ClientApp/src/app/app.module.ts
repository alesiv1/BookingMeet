import { BrowserModule } from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { GoogleAuthenticationComponent } from './google-authentication/google-authentication.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastrModule} from 'ngx-toastr';
import {Interceptor401Service} from './google-authentication/interceptor401.service';
import {checkIfUserIsAuthenticated} from './google-authentication/check-login-intializer';
import {AccountService} from './google-authentication/account.service';

@NgModule({
  declarations: [
    GoogleAuthenticationComponent,
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: GoogleAuthenticationComponent, pathMatch: 'full'},
      {path: 'counter', component: CounterComponent},
      {path: 'fetch-data', component: FetchDataComponent},
      {path: 'home', component: HomeComponent},
      {path: '**', redirectTo: '/' }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: Interceptor401Service, multi: true },
    { provide: APP_INITIALIZER, useFactory: checkIfUserIsAuthenticated, multi: true, deps: [AccountService]}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

