import { Injectable, Inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
import {ToastrService} from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private _isUserAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isUserAuthenticated: Observable<boolean> = this._isUserAuthenticatedSubject.asObservable();
  _baseUrl: string;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string)
  {
    this._baseUrl = baseUrl;
  }

  updateUserAuthenticationStatus(){
      return this.httpClient.get<boolean>(`${this._baseUrl}authentication/isAuthenticated`, { withCredentials: true }).pipe(tap(isAuthenticated => {
          this._isUserAuthenticatedSubject.next(isAuthenticated);
          if (!isAuthenticated && this.document.location.href != `${this._baseUrl}`) {
              this.document.location.href = `${this._baseUrl}`;
          }
      }));
  }

  setUserAsNotAuthenticated() {
    this._isUserAuthenticatedSubject.next(false);
  }

  login() {
    this.document.location.href = `${this._baseUrl}authentication/SignInWithGoogle`;
  }

  logout() {
    this.document.location.href = `${this._baseUrl}authentication/logout`;
  }

}
