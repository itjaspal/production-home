import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { DataQcView, DataQcCuttingView } from '../../_model/product-defect';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanDefectService } from '../../_service/scan-defect.service';

@Component({
  selector: 'app-scan-defect-remark',
  templateUrl: './scan-defect-remark.component.html',
  styleUrls: ['./scan-defect-remark.component.scss']
})
export class ScanDefectRemarkComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: DataQcView,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _defectSvc: ScanDefectService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: DataQcCuttingView = new DataQcCuttingView();
  // public searchModel: ScanReceiveSearchView = new ScanReceiveSearchView();
  // public model_scan: ScanReceiveFinView = new ScanReceiveFinView();  
  public datas: any = {};
  public item_no: any;
  public curr_time: any;


  async ngOnInit() {
    this.buildForm();
    var datePipe = new DatePipe("en-US");
    this.user = this._authSvc.getLoginUser();
    
    // this.model.item_no = this.data.item_no;
    this.model.por_no = this.data.por_no;
   
    
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      remark1: [null, []],
      remark2: [null, []],
      remark3: [null, []],
    });
  }


  async save() {
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    this.model.prod_code = this.data.prod_code;
    this.model.qc_process = "FG";
    this.model.build_type = this.user.branch.entity_code;
    this.model.user_id = this.user.username;
    this.model.item_no = this.data.item_no;
    console.log(this.model);
    
    this.datas = await this._defectSvc.EntryRemark(this.model);
    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
        
    this.dialogRef.close([]);
    

  }

  
  
  close() {
    this.dialogRef.close([]);
  }

}
