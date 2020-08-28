import { Component, OnInit } from '@angular/core';
import { MenuGroupService } from '../../_service/menu-group.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { BranchGroupView } from '../../_model/branchGroup';
import { MenuGroupView } from '../../_model/menugroup';

@Component({
  selector: 'app-menu-group-create',
  templateUrl: './menu-group-create.component.html',
  styleUrls: ['./menu-group-create.component.scss']
})
export class MenuGroupCreateComponent implements OnInit {

  constructor(
    private _menuGroupSvc: MenuGroupService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  public model: MenuGroupView = new MenuGroupView();
  public validationForm: FormGroup;

  async ngOnInit() {
     
    this.buildForm();
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      menuFunctionGroupId: [null, [Validators.required]],
      menuFunctionGroupName: [null, [Validators.required]],      
      orderDisplay:[null, [Validators.required]]
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._menuGroupSvc.create(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/menu-group');

  }

}
