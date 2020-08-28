import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../_service/menu.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from '../../_service/message.service';
import { MenuView } from '../../_model/menu';

@Component({
  selector: 'app-menu-update',
  templateUrl: './menu-update.component.html',
  styleUrls: ['./menu-update.component.scss']
})
export class MenuUpdateComponent implements OnInit {

  constructor(
    private _menuSvc: MenuService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    //private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  public model: MenuView = new MenuView();
  public validationForm: FormGroup;
  public code : string = undefined;
   
  async ngOnInit() {
   
    this.code = this._activateRoute.snapshot.params.menuFunctionId;
    console.log(this.code);
    this.buildForm();

    if (this.code != undefined) {
      this.model = await this._menuSvc.getInfo(this.code);
      console.log(this.model);
    }
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      menuFunctionId: [null, [Validators.required]],
      menuFunctionGroupId: [null, [Validators.required]],      
      menuFunctionName:[null, [Validators.required]],
      menuURL: [null, []],
      iconName: [null, []]
      //orderDisplay: [null, [Validators.required]]   
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._menuSvc.update(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/menu');

  }

}
