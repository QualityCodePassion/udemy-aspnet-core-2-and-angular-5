import { MemberEditComponent } from './../members/member-edit/member-edit.component';
import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent> {
    canDeactivate(component: MemberEditComponent) {
        if (component.childForm.dirty) {
            return confirm('Are you sure you want to leave this page? Any unsaved changes will be lost.');
        }
        return true;
    }
}