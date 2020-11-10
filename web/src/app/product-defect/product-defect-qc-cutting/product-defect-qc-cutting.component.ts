import { DataQcView, DataQcCuttingView } from './../../_model/product-defect';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ProductDefectService } from '../../_service/product-defect.service';
import { ItemSearchComponent } from '../item-search/item-search.component';


@Component({
  selector: 'app-product-defect-qc-cutting',
  templateUrl: './product-defect-qc-cutting.component.html',
  styleUrls: ['./product-defect-qc-cutting.component.scss']
})
export class ProductDefectQcCuttingComponent implements OnInit {

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
    this.item_no = await this._defectSvc.getItemNo(this.data.entity, this.data.por_no);
    this.model.qc_qty = this.data.qc_qty;
    this.model.qc_date = datePipe.transform(new Date(), 'dd/MM/yyyy').toString();
    this.model.qc_time = datePipe.transform(new Date(), 'hh:mm').toString();
    this.model.item_no = this.item_no;
    this.model.por_no = this.data.por_no;
    this.model.prod_code = this.data.prod_code;
    this.model.size_name = this.data.size_name;
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      qc_qty: [null, [Validators.required]],
      no_pass_qty: [null, [Validators.required]],
      width: [null, [Validators.required]],
      lenght: [null, [Validators.required]],
      remark1: [null, []],
      remark2: [null, []],
      remark3: [null, []],
    });
  }


  async save() {
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    this.model.prod_code = this.data.prod_code;
    this.model.qc_process = "CUTTING";
    this.model.build_type = this.user.branch.entity_code;
    this.model.user_id = this.user.username;
    console.log(this.model);
    
    this.datas = await this._defectSvc.DataQcCutting(this.model);
        
    this.dialogRef.close([]);
    

  }

  openSearchItemModal(p_por_no: string, _index: number = -1) {
    const dialogRef = this._dialog.open(ItemSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        por_no: p_por_no,
        entity: this.model.entity

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
    this.model.size_name = datas[0].size_name;
    this.model.item_no = datas[0].item_no;
    this.model.qc_process = datas[0].qc_process;
    this.model.qc_qty = datas[0].qc_qty;
    this.model.no_pass_qty = datas[0].no_pass_qty;
    this.model.width = datas[0].width;
    this.model.lenght = datas[0].lenght;
    this.model.remark1 = datas[0].remark1;
    this.model.remark2 = datas[0].remark2;
    this.model.remark3 = datas[0].remark3;
    this.model.user_id = this.user.username

  }

  close() {
    this.dialogRef.close([]);
  }

}
