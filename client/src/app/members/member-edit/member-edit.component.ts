import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { Member } from 'src/app/_modules/member';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  } //emito un mensaje si alguien esta editando el form de su perfil y se quiere ir de la page
  member: Member | undefined;
  user: User | null = null; //our user is null initialy and is the all interface

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => (this.user = user),
    }); //obtengo el usuario y lo igualo al user que estaba null
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if (!this.user) return; //si el user esta undefined o null detengo la ejecucion
    {
      this.memberService.getMember(this.user.username).subscribe({
        next: (member) => {
          this.member = member;
        },
      });
    }
  }

  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: (_) => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member); //reseteo el edit form para que una vez que guarde se borre el mensaje de que se modifico
      },
    });
  }
}
