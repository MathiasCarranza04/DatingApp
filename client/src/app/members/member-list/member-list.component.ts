import { Component, OnInit } from '@angular/core'
import { Member } from 'src/app/_modules/member'
import { MembersService } from 'src/app/_services/members.service'

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[] = [] //lo inicio vacio

  constructor(private memberService: MembersService) {}
  ngOnInit(): void {
    this.loadMembers()
  }

  loadMembers() {
    this.memberService.getMembers().subscribe({
      // prettier-ignore
      next: members => this.members = members //obtengo lista de members y la guardo en mi array de members
    })
  }
}
