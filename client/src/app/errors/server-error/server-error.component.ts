import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css'],
})
export class ServerErrorComponent implements OnInit {
  error: any; //este error deberia ser el que vuelve de la api, el object null reference

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error']; //obtengo el valor de la propiedad error del objecto state, viene del interceptor en el case 500
  }

  ngOnInit(): void {}
}
