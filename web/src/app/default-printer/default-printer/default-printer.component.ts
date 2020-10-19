import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { DisplayDefaultPrinterView, DefaultPrinterView } from '../../_model/default-printer';
import { AuthenticationService } from '../../_service/authentication.service';
import { DefaultPrinterService } from '../../_service/default-printer.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';


@Component({
  selector: 'app-default-printer',
  templateUrl: './default-printer.component.html',
  styleUrls: ['./default-printer.component.scss']
})
export class DefaultPrinterComponent implements OnInit {
  

  constructor(
    private _authSvc: AuthenticationService,
    private _defaultPrinterSvc: DefaultPrinterService,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _formBuilder: FormBuilder,
    private _router: Router,
  ) { }

  public user: any;
  public model: DisplayDefaultPrinterView = new DisplayDefaultPrinterView();
  public modelUpdate: DefaultPrinterView = new DefaultPrinterView();
  public ddlDefaultPrinter: any;
  public validationForm: FormGroup;

  ngOnInit() { 
    this.user = this._authSvc.getLoginUser();
    this.getDefaultPrinter();

    console.log(this.user)

    if(this.user.branch.entity_code == "HMJIT")
    {
      forkJoin([
        this._ddlSvc.getDdlDefaultPrinterJIT()
      
      ]).subscribe(result => {
        this.ddlDefaultPrinter = result[0];
    
      });
    }
    else
    {
      forkJoin([
        this._ddlSvc.getDdlDefaultPrinterSTK()
      
      ]).subscribe(result => {
        this.ddlDefaultPrinter = result[0];
    
      });
    }

    // forkJoin([
    //   this._ddlSvc.getDdlDefaultPrinterJIT()
    
    // ]).subscribe(result => {
    //   this.ddlDefaultPrinter = result[0];
  
    // });
    
    console.log(this.ddlDefaultPrinter);

    this.buildForm();
    
  }


  buildForm() {
    this.validationForm = this._formBuilder.group({
      serial_no: [null, [Validators.required]]
    });
}

  close() {
    this._router.navigateByUrl('/app/mobile-navigator');
  }

  async save() {
    var _valid = true;
    this.modelUpdate.user_id = this.user.username;
    //this.modelUpdate.mc_code = this.user.mc_code;

    console.log("this.modelUpdate.user_id: " + this.modelUpdate.user_id);
    //console.log("this.modelUpdate.mc_code: " + this.modelUpdate.mc_code);

    let res = await this._defaultPrinterSvc.update(this.modelUpdate); 
    

    await this._msgSvc.successPopup("ทำการบันทึกข้อมูลเรียบร้อยแล้ว");
    this.getDefaultPrinter();
    //this._router.navigateByUrl('defprinter');
  
  }


  async getDefaultPrinter() {
    this.model = await this._defaultPrinterSvc.getDefaultPrinter(this.user.username);
    console.log(this.model);
    
  }

  


}
