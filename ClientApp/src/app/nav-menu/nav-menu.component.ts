import {Component, OnInit} from '@angular/core';
import {AccountService} from '../google-authentication/account.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent  implements OnInit {

  isExpanded = false;
  subscription: Subscription;
  isAuthorize = false;

  constructor(private accountService:  AccountService) {}

  ngOnInit() {
    this.subscription = this.accountService.isUserAuthenticated.subscribe(isAuthenticated => {
      this.isAuthorize = isAuthenticated;
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
