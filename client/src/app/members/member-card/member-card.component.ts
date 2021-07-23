import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import {MembersService} from "../../_services/members.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css', '../../shared/shared.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member;
  constructor(private memberService: MembersService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe(() => {
      this.toastrService.success('You have liked ' + member.knownAs);
    });
  }

}
