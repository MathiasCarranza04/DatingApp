import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Dating app';

  constructor(private accountService: AccountService) {} //inyectamos servicio account

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user'); // la key user esta seteada en el accountservi linea 22
    if (!userString) return; //sino tenemos el usuario
    const user: User = JSON.parse(userString); //si tenemos el usuario del paso anterior lo parseamos
    this.accountService.setCurrentUser(user);
  }
}
