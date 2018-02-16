import { Pagination, PaginationResult } from './../../_models/pagination';
import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  // TODO this is the way the instructor retreives the user object,
  // but I must have skipped the lecture where he shored it. So I 
  // will come back and finish this later.
  //user: User = JSON.parse(localStorage.getItem('user'));
  pagination: Pagination;
  genderList = [{value: 'male', Display: 'Males'}, {value: 'female',  Display: 'Females'}];
  userParams: any = {};

  constructor(private userServive: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.setDefaults();
  }

  loadUsers() {
    this.userServive.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginationResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
      }, err => {
        this.alertify.error(err);
      }
    );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    console.log(this.pagination.currentPage);
    this.loadUsers();
  }

  resetFilters() {
    this.setDefaults();
    this.loadUsers();
  }

  setDefaults() {
    // Again since this is based on a course that was building a dataing
    // website for the project, it's assumed that the default is to display
    // users of the opposite sex.
    //this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
  }

}
