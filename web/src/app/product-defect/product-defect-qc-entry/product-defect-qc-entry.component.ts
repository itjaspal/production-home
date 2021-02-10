import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { DataQcView, DataQcEnrtyView } from '../../_model/product-defect';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ProductDefectService } from '../../_service/product-defect.service';
import { ItemEntrySearchComponent } from '../item-entry-search/item-entry-search.component';

@Component({
  selector: 'app-product-defect-qc-entry',
  templateUrl: './product-defect-qc-entry.component.html',
  styleUrls: ['./product-defect-qc-entry.component.scss']
})
export class ProductDefectQcEntryComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: DataQcView,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _defectSvc: ProductDefectService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  // public validationForm: FormGroup;
  public user: any;
  public model: DataQcEnrtyView = new DataQcEnrtyView();
  // public searchModel: ScanReceiveSearchView = new ScanReceiveSearchView();
  // public model_scan: ScanReceiveFinView = new ScanReceiveFinView();  
  public datas: any = {};
  public item_no: any;
  // public curr_time: any;
  public group_qc: any = {};
  public qty_chk : number = 0;

  async ngOnInit() {
    // this.buildForm();
    var datePipe = new DatePipe("en-US");
    this.user = this._authSvc.getLoginUser();
    
    this.qty_chk = this.data.qc_qty;
    this.model.qc_qty = 0;
    this.model.qc_date = datePipe.transform(new Date(), 'dd/MM/yyyy').toString();
    this.model.qc_time = datePipe.transform(new Date(), 'HH:mm').toString();
    this.model.item_no = this.item_no;
    this.model.por_no = this.data.por_no;
    this.model.prod_code = this.data.prod_code;

    this.group_qc = await this._defectSvc.getQcGroup(this.user.branch.entity_code);
    
    this.model.qc_process = "WIP";
    console.log(this.model.qc_process);
    this.item_no = await this._defectSvc.getItemQcEntryNo(this.model.entity, this.model.por_no , this.model.qc_process);
    this.model.item_no = this.item_no;
    
    console.log(this.group_qc);
  }

  

  async save() {
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    this.model.prod_code = this.data.prod_code;
    // this.model.qc_process = "CUTTING";
    this.model.build_type = this.user.branch.entity_code;
    this.model.user_id = this.user.username;
    this.model.datas = this.group_qc.datas;
    console.log(this.model);
    
    if(this.model.qc_qty > this.qty_chk)
    {
      this._msgSvc.warningPopup("จำนวนไม่ผ่านมากกว่าจำนวนสินค้า");
    }
    else
    {
      this.datas = await this._defectSvc.DataQcEntry(this.model);
    
      await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
      this.dialogRef.close([]);
    }
    
    

  }

  openSearchItemModal(p_por_no: string, _index: number = -1) {
    const dialogRef = this._dialog.open(ItemEntrySearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        por_no: p_por_no,
        entity: this.model.entity,
        qc_process : this.model.qc_process
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.length > 0) {
        this.add_data(result);
      }
    })
  }

  add_data(datas: any) {

    console.log(datas);
    this.model.entity = datas[0].entity;
    this.model.prod_code = datas[0].prod_code;
    this.model.por_no = datas[0].por_no;
    this.model.qc_date = datas[0].qc_date;
    this.model.qc_time = datas[0].qc_time;
    this.model.ref_no = datas[0].ref_no;
    
    this.model.item_no = datas[0].item_no;
    this.model.qc_process = datas[0].qc_process;
    this.model.qc_qty = datas[0].qc_qty;
    this.model.no_pass_qty = datas[0].no_pass_qty;
    
    this.model.remark1 = datas[0].remark1;
    this.model.remark2 = datas[0].remark2;
    this.model.remark3 = datas[0].remark3;
    this.model.remark4 = datas[0].remark4;
    this.model.remark5 = datas[0].remark5;
    this.model.remark6 = datas[0].remark6;
    this.model.user_id = this.user.username
    this.group_qc.datas = datas[0].groupdatas;

  }

  async radioTypeChange(value) {
    this.model.qc_process = value;
    console.log(this.model.qc_process);
    this.item_no = await this._defectSvc.getItemQcEntryNo(this.model.entity, this.model.por_no , this.model.qc_process);
    this.model.item_no = this.item_no;
  }

  close() {
    this.dialogRef.close([]);
   
  }


}
