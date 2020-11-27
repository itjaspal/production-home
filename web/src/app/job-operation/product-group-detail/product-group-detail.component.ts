import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProductGroupSearchView } from '../../_model/job-operation-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOperationStockService } from '../../_service/job-operation-stock.service';

@Component({
  selector: 'app-product-group-detail',
  templateUrl: './product-group-detail.component.html',
  styleUrls: ['./product-group-detail.component.scss']
})
export class ProductGroupDetailComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductGroupSearchView,
    private _fb: FormBuilder,
    private _jobSvc: JobOperationStockService,
    private cdr: ChangeDetectorRef,
    private _authSvc: AuthenticationService,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : ProductGroupSearchView  = new ProductGroupSearchView();
  //datas: any;
  datas: any = {};

  async ngOnInit() {
    var datePipe = new DatePipe("en-US");

    console.log(this.data);
    this.user = this._authSvc.getLoginUser();   
    this.model.entity_code = this.data.entity_code;
    this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString(); 
    this.model.wc_code = this.data.wc_code;
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    this.model.user_id = this.user.username;
    this.model.build_type = this.user.branch.entity_code;
    
    console.log(this.model);
    this.datas = await this._jobSvc.SearchSummaryProdcutGroup(this.model);
    console.log(this.datas);
  }

  close()
  {
    this.dialogRef.close([]);
  }

}
