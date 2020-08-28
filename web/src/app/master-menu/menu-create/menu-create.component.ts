import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../_service/menu.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { MenuView } from '../../_model/menu';

@Component({
  selector: 'app-menu-create',
  templateUrl: './menu-create.component.html',
  styleUrls: ['./menu-create.component.scss']
})
export class MenuCreateComponent implements OnInit {

  constructor(
    private _menuSvc: MenuService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  public model: MenuView = new MenuView();
  public validationForm: FormGroup;

  async ngOnInit() {
     
    this.buildForm();
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      menuFunctionId: [null, [Validators.required]],
      menuFunctionGroupId: [null, [Validators.required]],      
      menuFunctionName:[null, [Validators.required]],
      menuURL: [null, [Validators.required]],
      iconName: [null, []],
      orderDisplay: [null, [Validators.required]]
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._menuSvc.create(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/menu');

  }

}
