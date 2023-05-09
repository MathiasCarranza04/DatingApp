import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};

  //al constructor le inyecto diferentes servicios
  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => this.router.navigateByUrl('/members'),
      error: (error) => this.toastr.error(error.error),
    });
  }
  resetForm() {
    this.model.username = '';
    this.model.password = '';
  }

  logout() {
    this.accountService.logout(); //remuevo el user del local storage
    this.router.navigateByUrl('/'); //lo llevo a la home page
    this.resetForm(); //para limpiar los campos despues de desloguear
  }
}
