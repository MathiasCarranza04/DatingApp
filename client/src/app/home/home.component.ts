import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  registerMode = false;
  users: any;

  constructor() {}

  //OnInit permite añadir una inicializacion extra
  ngOnInit(): void {}

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  //cancelo el registro con el evento que me llega desde el register component
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
