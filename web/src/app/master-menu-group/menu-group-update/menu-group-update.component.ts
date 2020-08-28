import { Component, OnInit } from '@angular/core';
import { MenuGroupView } from '../../_model/menugroup';
import { MenuGroupService } from '../../_service/menu-group.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';

@Component({
  selector: 'app-menu-group-update',
  templateUrl: './menu-group-update.component.html',
  styleUrls: ['./menu-group-update.component.scss']
})
export class MenuGroupUpdateComponent implements OnInit {

  constructor(
    private _menuGroupSvc: MenuGroupService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    //private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  public model: MenuGroupView = new MenuGroupView();
  public validationForm: FormGroup;
  public code : string = undefined;
   
  async ngOnInit() {
   
    this.code = this._activateRoute.snapshot.params.menuFunctionGroupId;
    console.log(this.code);
    this.buildForm();

    if (this.code != undefined) {
      this.model = await this._menuGroupSvc.getInfo(this.code);
      console.log(this.model);
    }
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      menuFunctionGroupId: [null, []],
      menuFunctionGroupName: [null, [Validators.required]],  
      iconName: [null, []],
      orderDisplay : [null,[Validators.required]]    
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._menuGroupSvc.update(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/menu-group');

  }

}
