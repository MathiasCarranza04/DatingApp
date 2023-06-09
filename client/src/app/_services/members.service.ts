import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { environment } from 'src/environments/environment'
import { Member } from '../_modules/member'
import { map, of } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl
  members: Member[] = [] //si es mayor a 0 retorno los members desde el service

  constructor(private http: HttpClient) {}

  getMembers() {
    if (this.members.length > 0) return of(this.members)
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members
        return members
      }),
    )
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username) //pregunto si el username que llega por parametro esta en mi array
    if (member) return of(member)
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member)
  }
}
