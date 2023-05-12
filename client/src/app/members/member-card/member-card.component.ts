import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_modules/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent implements OnInit {
  //recibo los usuarios del componente padre, member-list

  @Input() member: Member = {} as Member; //como no tenemos el member aun casteamos el objecto vacio a member

  constructor() {}
  ngOnInit(): void {}
}
