import { UserService } from './../../_services/user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/User';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { AuthService } from '../../_services/auth.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  @ViewChild('memberForm') childForm: NgForm;

  constructor(private route: ActivatedRoute,
              private userServive: UserService,
              private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }


  onSubmit() {
    // This code is based on the example in Angular Forms, see https://angular.io/guide/forms
    this.userServive.updateUser(this.authService.getUserId(), this.user)
      .subscribe((res: any) => {
        this.alertify.success('Your changes were successfully changed');
        this.childForm.reset(this.user);
      }, err => {
        this.alertify.error('Could not connect to server to save changes! Check internet connection.');
        console.log('Could not connect to server to save changes because of following error: ' + err);
      }
    );
  }

}
