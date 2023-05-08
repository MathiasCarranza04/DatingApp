import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) {}

  //OnInit permite añadir una inicializacion extra
  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  // hago el get sobre el servicio del BE, el get retorna un observable
  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }

  //cancelo el registro con el evento que me llega desde el register component
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
