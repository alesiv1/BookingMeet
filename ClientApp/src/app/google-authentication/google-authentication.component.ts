import {Component, Inject, OnInit} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {HttpClient} from '@angular/common/http';
import {Subscription} from 'rxjs';
import {AccountService} from './account.service';

@Component({
  selector: 'app-google-authentication',
  templateUrl: './google-authentication.component.html',
  styleUrls: ['./google-authentication.component.css']
})
export class GoogleAuthenticationComponent implements OnInit {
  isUserAuthenticated = false;
  subscription: Subscription;
  userName: string;
  _baseUrl: string;

  constructor(private toast: ToastrService, private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private accountService:  AccountService) {
    this._baseUrl = baseUrl;
  }

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      this.isUserAuthenticated = isAuthenticated;
      if (this.isUserAuthenticated) {
        this.http.get(`${this._baseUrl}authentication/name`, { responseType: 'text', withCredentials: true }).subscribe(theName => {
          this.userName = theName;
        });
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  logout() {
    this.accountService.logout();
  }

  simulateFailedCall() {
    this.http.get(`${this._baseUrl}authentication/fail`).subscribe();
  }

  login() {
    this.accountService.login();
  }
}
