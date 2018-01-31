import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() cancelRegister = new EventEmitter();

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    // TODO Add error handling
    this.authService.register(this.model).subscribe( () => {
      this.alertify.success('Registration Successful');
    }, error => {
      this.alertify.error(error);
    });
  }

  cancelled() {
    this.cancelRegister.emit(false);
  }

}
