Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath


Namespace DCSGlobal.Rules.GeneratedDLL


    Public Class modFireRulesAddrVB


        Implements IDisposable

        Private disposedValue As Boolean ' To detect redundant calls

        Private _ConnectionString As String = String.Empty

        Private _Err As String = String.Empty

        Private _RuleResults As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)

        Private _RuleMsg As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

        Private _dr As DataRow

        Private _isBuilt As Boolean = False

        Private _isMessagesBuilt As Boolean = False
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            'Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub


        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then


                End If
            End If
            Me.disposedValue = True
        End Sub
        Dim g_sbAllRuleInsert As StringBuilder
        Dim g_resultAll As String = String.Empty


        Public WriteOnly Property ResultAll As String


            Set(value As String)
                g_resultAll = value
            End Set
        End Property


        Public WriteOnly Property ConnectionString As String


            Set(value As String)
                _ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property DR As DataRow



            Set(value As DataRow)

                _dr = value

            End Set

        End Property

        Public ReadOnly Property RuleResults As Dictionary(Of Integer, Integer)

            Get

                Return _RuleResults

            End Get

        End Property

        Public ReadOnly Property RuleMessages As Dictionary(Of Integer, String)

            Get

                Return _RuleMsg

            End Get

        End Property










        Private _patient_audit_trail_id As System.Int32 = 0
        Private _patient_id As System.Int32 = 0
        Private _patient_number As System.String = String.Empty
        Private _med_record_number As System.String = String.Empty
        Private _patient_last_name As System.String = String.Empty
        Private _patient_first_name As System.String = String.Empty
        Private _patient_middle_initial As System.String = String.Empty
        Private _patient_title As System.String = String.Empty
        Private _patient_mother_maiden_name As System.String = String.Empty
        Private _patient_birth_date As System.String = String.Empty
        Private _patient_sex As System.String = String.Empty
        Private _patient_alias As System.String = String.Empty
        Private _patient_street_address As System.String = String.Empty
        Private _patient_address2 As System.String = String.Empty
        Private _patient_city As System.String = String.Empty
        Private _patient_state As System.String = String.Empty
        Private _patient_zip_code As System.String = String.Empty
        Private _patient_country_code As System.String = String.Empty
        Private _patient_home_phone_number As System.String = String.Empty
        Private _patient_email As System.String = String.Empty
        Private _patient_language As System.String = String.Empty
        Private _patient_marital_status As System.String = String.Empty
        Private _patient_religion_code As System.String = String.Empty
        Private _patient_ssn As System.String = String.Empty
        Private _patient_class As System.String = String.Empty
        Private _patient_location As System.String = String.Empty
        Private _admit_type As System.String = String.Empty
        Private _attending_doctor_number As System.String = String.Empty
        Private _attending_doctor_name As System.String = String.Empty
        Private _referring_doctor_number As System.String = String.Empty
        Private _referring_doctor_name As System.String = String.Empty
        Private _consulting_doctor_number As System.String = String.Empty
        Private _consulting_doctor_name As System.String = String.Empty
        Private _hospital_service_code As System.String = String.Empty
        Private _patient_temp_location As System.String = String.Empty
        Private _admit_source As System.String = String.Empty
        Private _admitting_doctor_number As System.String = String.Empty
        Private _admitting_doctor_name As System.String = String.Empty
        Private _account_type As System.String = String.Empty
        Private _account_class As System.String = String.Empty
        Private _servicing_facility As System.String = String.Empty
        Private _admitting_date As System.String = String.Empty
        Private _discharge_date As System.String = String.Empty
        Private _diagnosis_code As System.String = String.Empty
        Private _diagnosis_desc As System.String = String.Empty
        Private _drg_code As System.String = String.Empty
        Private _accident_date_time As System.String = String.Empty
        Private _accident_code As System.String = String.Empty
        Private _occurrence_codes As System.String = String.Empty
        Private _occurrence_code_dates As System.String = String.Empty
        Private _guarantor_last_name As System.String = String.Empty
        Private _guarantor_first_name As System.String = String.Empty
        Private _gurantor_middle_initial As System.String = String.Empty
        Private _guarantor_spouse_name As System.String = String.Empty
        Private _gurantor_street_address As System.String = String.Empty
        Private _gurantor_address2 As System.String = String.Empty
        Private _gurantor_city As System.String = String.Empty
        Private _gurantor_state As System.String = String.Empty
        Private _gurantor_zip_code As System.String = String.Empty
        Private _guarantor_country_code As System.String = String.Empty
        Private _guarantor_home_phone_num As System.String = String.Empty
        Private _guarantor_email As System.String = String.Empty
        Private _guarantor_dob As System.String = String.Empty
        Private _gurantor_dob As System.String = String.Empty
        Private _guarantor_sex As System.String = String.Empty
        Private _guarantor_type As System.String = String.Empty
        Private _guarantor_relationship As System.String = String.Empty
        Private _gurantor_ssn As System.String = String.Empty
        Private _gurantor_emp_name As System.String = String.Empty
        Private _gurantor_emp_address As System.String = String.Empty
        Private _gurantor_emp_address2 As System.String = String.Empty
        Private _gurantor_emp_city As System.String = String.Empty
        Private _gurantor_emp_state As System.String = String.Empty
        Private _gurantor_emp_zip_code As System.String = String.Empty
        Private _gurantor_emp_phone_number As System.String = String.Empty
        Private _gurantor_emp_phone_extension As System.String = String.Empty
        Private _gurantor_emp_country_code As System.String = String.Empty
        Private _contact_last_name As System.String = String.Empty
        Private _contact_first_name As System.String = String.Empty
        Private _contact_middle_initial As System.String = String.Empty
        Private _contact_relationship_to_pat As System.String = String.Empty
        Private _contact_street_address As System.String = String.Empty
        Private _contact_address2 As System.String = String.Empty
        Private _contact_city As System.String = String.Empty
        Private _contact_state As System.String = String.Empty
        Private _nearest_relative_name As System.String = String.Empty
        Private _nearest_relative_phone As System.String = String.Empty
        Private _nearest_relative_rel As System.String = String.Empty
        Private _contact_zip_code As System.String = String.Empty
        Private _contact_country_code As System.String = String.Empty
        Private _contact_home_phone_num As System.String = String.Empty
        Private _contact_role As System.String = String.Empty
        Private _pri_ins_number As System.String = String.Empty
        Private _pri_ins_name As System.String = String.Empty
        Private _pri_ins_address As System.String = String.Empty
        Private _pri_address_2 As System.String = String.Empty
        Private _pri_ins_city As System.String = String.Empty
        Private _pri_ins_state As System.String = String.Empty
        Private _pri_ins_zip_code As System.String = String.Empty
        Private _pri_ins_country_code As System.String = String.Empty
        Private _pri_ins_contact_name As System.String = String.Empty
        Private _pri_ins_phone_number As System.String = String.Empty
        Private _pri_ins_group_name As System.String = String.Empty
        Private _pri_ins_group_number As System.String = String.Empty
        Private _pri_insured_group_emp_id As System.String = String.Empty
        Private _pri_insured_group_emp_name As System.String = String.Empty
        Private _pri_ins_plan_effective_date As System.String = String.Empty
        Private _pri_ins_plan_expiration_date As System.String = String.Empty
        Private _pri_ins_plan_type As System.String = String.Empty
        Private _pri_insured_last_name As System.String = String.Empty
        Private _pri_insured_first_name As System.String = String.Empty
        Private _pri_insured_relationship As System.String = String.Empty
        Private _pri_insured_dob As System.String = String.Empty
        Private _insured_dob As System.String = String.Empty
        Private _pri_insured_address As System.String = String.Empty
        Private _pri_insured_address2 As System.String = String.Empty
        Private _pri_insured_city As System.String = String.Empty
        Private _pri_insured_state As System.String = String.Empty
        Private _pri_insured_zip As System.String = String.Empty
        Private _pri_cob_priority As System.String = String.Empty
        Private _pri_date_of_verification As System.String = String.Empty
        Private _pri_name_of_verifier As System.String = String.Empty
        Private _pri_ins_policy_number As System.String = String.Empty
        Private _pri_insured_sex As System.String = String.Empty
        Private _pri_insured_emp_address As System.String = String.Empty
        Private _pri_insured_emp_address2 As System.String = String.Empty
        Private _pri_insured_emp_city As System.String = String.Empty
        Private _pri_insured_emp_state As System.String = String.Empty
        Private _pri_insured_emp_zip_code As System.String = String.Empty
        Private _pri_ins_verification_status As System.String = String.Empty
        Private _pri_insured_employee_id As System.String = String.Empty
        Private _pri_insured_ssn As System.String = String.Empty
        Private _pri_insured_emp_name As System.String = String.Empty
        Private _pri_ins_coverage_num As System.String = String.Empty
        Private _sec_plan_number As System.String = String.Empty
        Private _sec_ins_number As System.String = String.Empty
        Private _sec_ins_name As System.String = String.Empty
        Private _sec_ins_address As System.String = String.Empty
        Private _sec_address_2 As System.String = String.Empty
        Private _sec_ins_city As System.String = String.Empty
        Private _sec_ins_state As System.String = String.Empty
        Private _sec_ins_zip_code As System.String = String.Empty
        Private _sec_ins_country_code As System.String = String.Empty
        Private _sec_ins_contact_name As System.String = String.Empty
        Private _sec_ins_phone_number As System.String = String.Empty
        Private _sec_ins_group_name As System.String = String.Empty
        Private _sec_ins_group_number As System.String = String.Empty
        Private _sec_insured_group_emp_id As System.String = String.Empty
        Private _sec_insured_group_emp_name As System.String = String.Empty
        Private _sec_ins_plan_effective_date As System.String = String.Empty
        Private _sec_ins_plan_expiration_date As System.String = String.Empty
        Private _sec_ins_plan_type As System.String = String.Empty
        Private _sec_insured_last_name As System.String = String.Empty
        Private _sec_insured_first_name As System.String = String.Empty
        Private _sec_insured_relationship As System.String = String.Empty
        Private _sec_insured_dob As System.String = String.Empty
        Private _sec_insured_address As System.String = String.Empty
        Private _sec_insured_address2 As System.String = String.Empty
        Private _sec_insured_city As System.String = String.Empty
        Private _sec_insured_state As System.String = String.Empty
        Private _sec_insured_zip As System.String = String.Empty
        Private _sec_cob_priority As System.String = String.Empty
        Private _sec_date_of_verification As System.String = String.Empty
        Private _sec_name_of_verifier As System.String = String.Empty
        Private _sec_ins_policy_number As System.String = String.Empty
        Private _sec_insured_sex As System.String = String.Empty
        Private _sec_insured_emp_address As System.String = String.Empty
        Private _sec_insured_emp_address2 As System.String = String.Empty
        Private _sec_insured_emp_city As System.String = String.Empty
        Private _sec_insured_emp_state As System.String = String.Empty
        Private _sec_insured_emp_zip_code As System.String = String.Empty
        Private _sec_ins_verification_status As System.String = String.Empty
        Private _sec_insured_employee_id As System.String = String.Empty
        Private _sec_insured_ssn As System.String = String.Empty
        Private _sec_insured_employer_name As System.String = String.Empty
        Private _ter_plan_number As System.String = String.Empty
        Private _ter_ins_number As System.String = String.Empty
        Private _ter_ins_name As System.String = String.Empty
        Private _ter_ins_address As System.String = String.Empty
        Private _ter_address_2 As System.String = String.Empty
        Private _ter_ins_city As System.String = String.Empty
        Private _ter_ins_state As System.String = String.Empty
        Private _ter_ins_zip_code As System.String = String.Empty
        Private _ter_ins_country_code As System.String = String.Empty
        Private _ter_ins_contact_name As System.String = String.Empty
        Private _ter_ins_phone_number As System.String = String.Empty
        Private _ter_ins_group_name As System.String = String.Empty
        Private _ter_ins_group_number As System.String = String.Empty
        Private _ter_insured_group_emp_id As System.String = String.Empty
        Private _ter_insured_group_emp_name As System.String = String.Empty
        Private _ter_ins_plan_effective_date As System.String = String.Empty
        Private _ter_ins_plan_expiration_date As System.String = String.Empty
        Private _ter_ins_plan_type As System.String = String.Empty
        Private _ter_insured_last_name As System.String = String.Empty
        Private _ter_insured_first_name As System.String = String.Empty
        Private _ter_insured_relationship As System.String = String.Empty
        Private _ter_insured_dob As System.String = String.Empty
        Private _ter_insured_address As System.String = String.Empty
        Private _ter_insured_address2 As System.String = String.Empty
        Private _ter_insured_city As System.String = String.Empty
        Private _ter_insured_state As System.String = String.Empty
        Private _ter_insured_zip As System.String = String.Empty
        Private _ter_cob_priority As System.String = String.Empty
        Private _ter_date_of_verification As System.String = String.Empty
        Private _ter_name_of_verifier As System.String = String.Empty
        Private _ter_ins_policy_number As System.String = String.Empty
        Private _ter_insured_sex As System.String = String.Empty
        Private _ter_insured_emp_address As System.String = String.Empty
        Private _ter_insured_emp_address2 As System.String = String.Empty
        Private _ter_insured_emp_city As System.String = String.Empty
        Private _ter_insured_emp_state As System.String = String.Empty
        Private _ter_insured_emp_zip_code As System.String = String.Empty
        Private _ter_ins_verification_status As System.String = String.Empty
        Private _ter_insured_employee_id As System.String = String.Empty
        Private _ter_insured_ssn As System.String = String.Empty
        Private _ter_insured_employer_name As System.String = String.Empty
        Private _oth_plan_number As System.String = String.Empty
        Private _oth_name As System.String = String.Empty
        Private _oth_address As System.String = String.Empty
        Private _oth_address_2 As System.String = String.Empty
        Private _oth_ins_city As System.String = String.Empty
        Private _oth_ins_state As System.String = String.Empty
        Private _oth_ins_zip_code As System.String = String.Empty
        Private _oth_ins_country_code As System.String = String.Empty
        Private _oth_ins_contact_name As System.String = String.Empty
        Private _oth_ins_phone_number As System.String = String.Empty
        Private _oth_group_name As System.String = String.Empty
        Private _oth_group_number As System.String = String.Empty
        Private _oth_insured_group_emp_id As System.String = String.Empty
        Private _oth_insured_group_emp_name As System.String = String.Empty
        Private _oth_ins_plan_effective_date As System.String = String.Empty
        Private _oth_ins_plan_expiration_date As System.String = String.Empty
        Private _oth_ins_plan_type As System.String = String.Empty
        Private _oth_insured_last_name As System.String = String.Empty
        Private _oth_insured_first_name As System.String = String.Empty
        Private _oth_insured_relationship As System.String = String.Empty
        Private _oth_insured_dob As System.String = String.Empty
        Private _oth_insured_address As System.String = String.Empty
        Private _oth_insured_address2 As System.String = String.Empty
        Private _oth_insured_city As System.String = String.Empty
        Private _oth_insured_state As System.String = String.Empty
        Private _oth_insured_zip As System.String = String.Empty
        Private _oth_cob_priority As System.String = String.Empty
        Private _oth_date_of_verification As System.String = String.Empty
        Private _oth_name_of_verifier As System.String = String.Empty
        Private _oth_ins_policy_number As System.String = String.Empty
        Private _oth_insured_sex As System.String = String.Empty
        Private _oth_insured_emp_address As System.String = String.Empty
        Private _oth_insured_emp_address2 As System.String = String.Empty
        Private _oth_insured_emp_city As System.String = String.Empty
        Private _oth_insured_emp_state As System.String = String.Empty
        Private _oth_insured_emp_zip_code As System.String = String.Empty
        Private _oth_ins_verification_status As System.String = String.Empty
        Private _oth_insured_employee_id As System.String = String.Empty
        Private _oth_insured_ssn As System.String = String.Empty
        Private _oth_insured_employer_name As System.String = String.Empty
        Private _client_facility_code As System.String = String.Empty
        Private _create_date As System.String = String.Empty
        Private _create_time As System.String = String.Empty
        Private _source_file As System.String = String.Empty
        Private _system_type As System.String = String.Empty
        Private _sub_system As System.String = String.Empty
        Private _update_flag As System.Int32 = 0
        Private _ctvision_pt As System.String = String.Empty
        Private _ctvision_fc As System.String = String.Empty
        Private _id As System.Decimal = 0
        Private _event_type As System.String = String.Empty
        Private _event_detail As System.String = String.Empty
        Private _pat_hosp_code As System.String = String.Empty
        Private _operator_id As System.String = String.Empty
        Private _workstation_id As System.String = String.Empty
        Private _event_datetime As System.String = String.Empty
        Private _message_datetime As System.String = String.Empty
        Private _total_charge_amount As System.String = String.Empty
        Private _audit_flag As System.String = String.Empty
        Private _pid As System.Int32 = 0
        Private _patient_work_phone_num As System.String = String.Empty
        Private _patient_other_phone_num As System.String = String.Empty
        Private _patient_status As System.String = String.Empty
        Private _patient_pre_reg As System.String = String.Empty
        Private _patient_emp_code As System.String = String.Empty
        Private _patient_emp_name As System.String = String.Empty
        Private _patient_emp_status As System.String = String.Empty
        Private _patient_emp_phone_num As System.String = String.Empty
        Private _contact_work_phone_num As System.String = String.Empty
        Private _contact_other_phone_num As System.String = String.Empty
        Private _guarantor_other_phone_num As System.String = String.Empty
        Private _guarantor_occupation As System.String = String.Empty
        Private _guarantor_emp_code As System.String = String.Empty
        Private _nebo_patient_id As System.String = String.Empty
        Private _nebo_guarantor_id As System.String = String.Empty
        Private _pri_ins_treat_auth_id As System.String = String.Empty
        Private _pri_ins_bc_plan_code As System.String = String.Empty
        Private _sec_ins_bc_plan_code As System.String = String.Empty
        Private _ter_ins_bc_plan_code As System.String = String.Empty
        Private _pri_insured_home_phone_num As System.String = String.Empty
        Private _pri_insured_other_phone_num As System.String = String.Empty
        Private _pri_insured_emp_phone_num As System.String = String.Empty
        Private _sec_ins_treat_auth_id As System.String = String.Empty
        Private _sec_insured_home_phone_num As System.String = String.Empty
        Private _sec_insured_other_phone_num As System.String = String.Empty
        Private _sec_insured_emp_phone_num As System.String = String.Empty
        Private _ter_ins_treat_auth_id As System.String = String.Empty
        Private _ter_insured_home_phone_num As System.String = String.Empty
        Private _ter_insured_other_phone_num As System.String = String.Empty
        Private _ter_insured_emp_phone_num As System.String = String.Empty
        Private _oth_ins_treat_auth_id As System.String = String.Empty
        Private _oth_insured_home_phone_num As System.String = String.Empty
        Private _pri_nebo_field As System.String = String.Empty
        Private _sec_nebo_field As System.String = String.Empty
        Private _ter_nebo_field As System.String = String.Empty
        Private _oth_nebo_field As System.String = String.Empty
        Private _name_aka As System.String = String.Empty
        Private _special_guest As System.String = String.Empty
        Private _day_phone_num As System.String = String.Empty
        Private _primary_care_physician As System.String = String.Empty
        Private _referring_physician As System.String = String.Empty
        Private _religion As System.String = String.Empty
        Private _race As System.String = String.Empty
        Private _advanced_directive As System.String = String.Empty
        Private _smoking_education As System.String = String.Empty
        Private _msp_form_detail As System.String = String.Empty
        Private _medicare_plan As System.String = String.Empty
        Private _ppo_hmo As System.String = String.Empty
        Private _workers_comp_detail As System.String = String.Empty
        Private _mva As System.String = String.Empty
        Private _chief_complaint As System.String = String.Empty
        Private _charity_care As System.String = String.Empty
        Private _plan_expiration As System.String = String.Empty
        Private _referral_num As System.String = String.Empty
        Private _primary_sub_addres As System.String = String.Empty
        Private _recipient_num As System.String = String.Empty
        Private _ins_mail_claim As System.String = String.Empty
        Private _patient_alert As System.String = String.Empty
        Private _authorization_num As System.String = String.Empty
        Private _copay As System.String = String.Empty
        Private _ins_verification As System.String = String.Empty
        Private _ins_comp_name As System.String = String.Empty
        Private _region_code As System.String = String.Empty
        Private _division_code As System.String = String.Empty
        Private _facility_code As System.String = String.Empty
        Private _msp_error As System.String = String.Empty
        Private _patient_type As System.String = String.Empty
        Private _npp As System.String = String.Empty
        Private _npp_date As System.String = String.Empty
        Private _orgon_donor As System.String = String.Empty
        Private _gurantor_street_address2 As System.String = String.Empty
        Private _ad_date As System.String = String.Empty
        Private _financial_class As System.String = String.Empty
        Private _roi_consent As System.String = String.Empty
        Private _staff_alert As System.String = String.Empty
        Private _mcd_days As System.String = String.Empty
        Private _source As System.String = String.Empty
        Private _pri_ins_type As System.String = String.Empty
        Private _sec_ins_type As System.String = String.Empty
        Private _ter_ins_type As System.String = String.Empty
        Private _oth_ins_type As System.String = String.Empty
        Private _medicare_scanned As System.String = String.Empty
        Private _order_scanned As System.String = String.Empty
        Private _pri_insured_emp_status As System.String = String.Empty
        Private _pri_policy_type As System.String = String.Empty
        Private _pri_medicare_hic As System.String = String.Empty
        Private _pri_insured_country_code As System.String = String.Empty
        Private _pri_insured_emp_country_code As System.String = String.Empty
        Private _sec_insured_emp_status As System.String = String.Empty
        Private _sec_policy_type As System.String = String.Empty
        Private _sec_medicare_hic As System.String = String.Empty
        Private _sec_insured_country_code As System.String = String.Empty
        Private _sec_insured_emp_country_code As System.String = String.Empty
        Private _ter_insured_emp_status As System.String = String.Empty
        Private _ter_policy_type As System.String = String.Empty
        Private _ter_medicare_hic As System.String = String.Empty
        Private _ter_insured_country_code As System.String = String.Empty
        Private _ter_insured_emp_country_code As System.String = String.Empty
        Private _oth_insured_emp_status As System.String = String.Empty
        Private _oth_policy_type As System.String = String.Empty
        Private _oth_medicare_hic As System.String = String.Empty
        Private _oth_insured_country_code As System.String = String.Empty
        Private _oth_insured_emp_country_code As System.String = String.Empty
        Private _pri_ins_ded As System.String = String.Empty
        Private _pri_ins_oopd As System.String = String.Empty
        Private _sec_ins_ded As System.String = String.Empty
        Private _sec_ins_oopd As System.String = String.Empty
        Private _ter_ins_ded As System.String = String.Empty
        Private _ter_ins_oopd As System.String = String.Empty
        Private _oth_ins_ded As System.String = String.Empty
        Private _patient_race As System.String = String.Empty
        Private _patient_emp_address As System.String = String.Empty
        Private _patient_emp_address2 As System.String = String.Empty
        Private _patient_emp_city As System.String = String.Empty
        Private _patient_emp_state As System.String = String.Empty
        Private _patient_emp_zip_code As System.String = String.Empty
        Private _pri_insured_us_citizenship As System.String = String.Empty
        Private _pri_insured_race As System.String = String.Empty
        Private _pri_insured_marital_status As System.String = String.Empty
        Private _admission_priority As System.String = String.Empty
        Private _family_doctor_name As System.String = String.Empty
        Private _family_doctor_number As System.String = String.Empty
        Private _from_date As System.String = String.Empty
        Private _thru_date As System.String = String.Empty
        Private _actual_los As System.String = String.Empty
        Private _adt_comment As System.String = String.Empty
        Private _arrived_by As System.String = String.Empty
        Private _diag2 As System.String = String.Empty
        Private _diag3 As System.String = String.Empty
        Private _diag4 As System.String = String.Empty
        Private _diag5 As System.String = String.Empty
        Private _diag6 As System.String = String.Empty
        Private _last_hosp_from_date As System.String = String.Empty
        Private _last_hosp_thru_date As System.String = String.Empty
        Private _last_hosp_hospital As System.String = String.Empty
        Private _room_location As System.String = String.Empty
        Private _bed_location As System.String = String.Empty
        Private _rooms_accom As System.String = String.Empty
        Private _requested_accom As System.String = String.Empty
        Private _room_rate_accom As System.String = String.Empty
        Private _fin_con_num As System.String = String.Empty
        Private _fin_con_query As System.String = String.Empty
        Private _fin_con_resp As System.String = String.Empty
        Private _pri_not_num As System.String = String.Empty
        Private _pri_not_query As System.String = String.Empty
        Private _pri_not_resp As System.String = String.Empty
        Private _date_num As System.String = String.Empty
        Private _date_query As System.String = String.Empty
        Private _date_resp As System.String = String.Empty
        Private _phi_dis_num As System.String = String.Empty
        Private _phi_dis_query As System.String = String.Empty
        Private _phi_dis_resp As System.String = String.Empty
        Private _res_par_num As System.String = String.Empty
        Private _res_par_query As System.String = String.Empty
        Private _res_par_resp As System.String = String.Empty
        Private _date_sig_num As System.String = String.Empty
        Private _date_sig_query As System.String = String.Empty
        Private _date_sig_resp As System.String = String.Empty
        Private _ack_num As System.String = String.Empty
        Private _ack_query As System.String = String.Empty
        Private _ack_resp As System.String = String.Empty
        Private _admorl_num As System.String = String.Empty
        Private _admorl_query As System.String = String.Empty
        Private _admorl_resp As System.String = String.Empty
        Private _prior_location As System.String = String.Empty
        Private _condition_codes As System.String = String.Empty
        Private _coa_scanned As System.String = String.Empty
        Private _er_slip_scanned As System.String = String.Empty
        Private _wc_scanned As System.String = String.Empty
        Private _discharge_disposition As System.String = String.Empty
        Private _pri_status As System.String = String.Empty
        Private _sec_status As System.String = String.Empty
        Private _ter_status As System.String = String.Empty
        Private _oth_status As System.String = String.Empty
        Private _validation_code As System.String = String.Empty
        Private _address_dpv As System.String = String.Empty
        Private _provider_source As System.String = String.Empty
        Private _homeless As System.String = String.Empty
        Private _nonnetwkphys As System.String = String.Empty
        Private _coa_signed As System.String = String.Empty
        Private _admission_status As System.String = String.Empty
        Private _transfer_fac_code As System.String = String.Empty
        Private _discharge_letter As System.String = String.Empty
        Private _npp_form As System.String = String.Empty
        Private _mspq1 As System.String = String.Empty
        Private _mspq2 As System.String = String.Empty
        Private _mspq3 As System.String = String.Empty
        Private _mspq4 As System.String = String.Empty
        Private _mspq5 As System.String = String.Empty
        Private _mspq6 As System.String = String.Empty
        Private _mspq7 As System.String = String.Empty
        Private _mspq8 As System.String = String.Empty
        Private _mspq9 As System.String = String.Empty
        Private _mspq10 As System.String = String.Empty
        Private _mspq11 As System.String = String.Empty
        Private _mspq12 As System.String = String.Empty
        Private _mspq13 As System.String = String.Empty
        Private _mspq14 As System.String = String.Empty
        Private _mspq15 As System.String = String.Empty
        Private _mspq16 As System.String = String.Empty
        Private _mspq17 As System.String = String.Empty
        Private _mspq18 As System.String = String.Empty
        Private _mspq19 As System.String = String.Empty
        Private _mspq20 As System.String = String.Empty
        Private _mspq21 As System.String = String.Empty
        Private _authorization_service_primary As System.String = String.Empty
        Private _cpt1 As System.String = String.Empty
        Private _cpt2 As System.String = String.Empty
        Private _cpt3 As System.String = String.Empty
        Private _cpt4 As System.String = String.Empty
        Private _medicare_hic As System.String = String.Empty
        Private _bic_number As System.String = String.Empty
        Private _ccs_number As System.String = String.Empty
        Private _tricare_number As System.String = String.Empty
        Private _workerscomp_number As System.String = String.Empty
        Private _certificate_number As System.String = String.Empty
        Private _hmo_patientid As System.String = String.Empty
        Private _medicalgroup1 As System.String = String.Empty
        Private _healthplanname As System.String = String.Empty
        Private _encounter_loc As System.String = String.Empty
        Private _payor1_key As System.String = String.Empty
        Private _payor2_key As System.String = String.Empty
        Private _payor3_key As System.String = String.Empty
        Private _casekey As System.String = String.Empty
        Private _encounterkey As System.String = String.Empty
        Private _advanced_directive_file As System.String = String.Empty
        Private _rights_doc As System.String = String.Empty
        Private _patient_subscriber As System.String = String.Empty
        Private _team As System.String = String.Empty
        Private _broughtby As System.String = String.Empty
        Private _aka_lname As System.String = String.Empty
        Private _eligibilityflag1 As System.String = String.Empty
        Private _eligibilityflag2 As System.String = String.Empty
        Private _eligibilityflag3 As System.String = String.Empty
        Private _Medical_Benefits As System.String = String.Empty
        Private _Medicare_Benefits As System.String = String.Empty
        Private _visit_sequence As System.String = String.Empty
        Private _authorization_service_secondary As System.String = String.Empty
        Private _authorization_service_tertiary As System.String = String.Empty
        Private _nurse_unit As System.String = String.Empty
        Private _newborn_indicator As System.String = String.Empty
        Private _cob_date As System.String = String.Empty
        Private _referring_doctor_address As System.String = String.Empty
        Private _referring_doctor_city As System.String = String.Empty
        Private _referring_doctor_state As System.String = String.Empty
        Private _referring_doctor_zip As System.String = String.Empty
        Private _referring_doctor_phone As System.String = String.Empty
        Private _Primary_doctor_Id As System.String = String.Empty
        Private _Primary_doctor_LastName As System.String = String.Empty
        Private _Primary_doctor_FirstName As System.String = String.Empty
        Private _Primary_doctor_MiddleName As System.String = String.Empty
        Private _primary_doctor_address As System.String = String.Empty
        Private _primary_doctor_city As System.String = String.Empty
        Private _primary_doctor_state As System.String = String.Empty
        Private _primary_doctor_zip As System.String = String.Empty
        Private _primary_doctor_phone As System.String = String.Empty
        Private _outpatient_verify_flag As System.String = String.Empty
        Private _encounter_data_verify As System.String = String.Empty
        Private _retirement_date As System.String = String.Empty
        Private _patient_location2 As System.String = String.Empty
        Private _patient_location3 As System.String = String.Empty
        Private _patient_location4 As System.String = String.Empty
        Private _patient_location5 As System.String = String.Empty
        Private _pri_ins_benefit_plan As System.String = String.Empty
        Private _sec_ins_benefit_plan As System.String = String.Empty
        Private _ter_ins_benefit_plan As System.String = String.Empty
        Private _oth_ins_benefit_plan As System.String = String.Empty
        Private _quick_reg_flag As System.String = String.Empty
        Private _guarantor_number As System.String = String.Empty
        Private _patient_occupation As System.String = String.Empty
        Private _pri_insured_middle_initial As System.String = String.Empty
        Private _sec_insured_middle_initial As System.String = String.Empty
        Private _ter_insured_middle_initial As System.String = String.Empty
        Private _pri_insured_suffix As System.String = String.Empty
        Private _sec_insured_suffix As System.String = String.Empty
        Private _ter_insured_suffix As System.String = String.Empty
        Private _patient_county_code As System.String = String.Empty
        Private _mn_required As System.String = String.Empty
        Private _mn_completed As System.String = String.Empty
        Private _nearest_relative_street_address As System.String = String.Empty
        Private _nearest_relative_address2 As System.String = String.Empty
        Private _nearest_relative_city As System.String = String.Empty
        Private _nearest_relative_state As System.String = String.Empty
        Private _nearest_relative_zip_code As System.String = String.Empty
        Private _nearest_relative_country_code As System.String = String.Empty
        Private _pri_payor_subscriber_name_last As System.String = String.Empty
        Private _pri_payor_subscriber_name_first As System.String = String.Empty
        Private _pri_payor_subscriber_name_middle As System.String = String.Empty
        Private _pri_payor_subscriber_id As System.String = String.Empty
        Private _pri_payor_susbcriber_dob As System.String = String.Empty
        Private _pri_payor_subscriber_gender As System.String = String.Empty
        Private _pri_payor_plan_type As System.String = String.Empty
        Private _sec_payor_subscriber_name_last As System.String = String.Empty
        Private _sec_payor_subscriber_name_first As System.String = String.Empty
        Private _sec_payor_subscriber_name_middle As System.String = String.Empty
        Private _sec_payor_subscriber_id As System.String = String.Empty
        Private _sec_payor_susbcriber_dob As System.String = String.Empty
        Private _sec_payor_subscriber_gender As System.String = String.Empty
        Private _sec_payor_plan_type As System.String = String.Empty
        Private _ter_payor_subscriber_name_last As System.String = String.Empty
        Private _ter_payor_subscriber_name_first As System.String = String.Empty
        Private _ter_payor_subscriber_name_middle As System.String = String.Empty
        Private _ter_payor_subscriber_id As System.String = String.Empty
        Private _ter_payor_susbcriber_dob As System.String = String.Empty
        Private _ter_payor_subscriber_gender As System.String = String.Empty
        Private _ter_payor_plan_type As System.String = String.Empty
        Private _pri_payor_code As System.String = String.Empty
        Private _sec_payor_code As System.String = String.Empty
        Private _ter_payor_code As System.String = String.Empty
        Private _pri_mcra_inactive As System.String = String.Empty
        Private _sec_mcra_inactive As System.String = String.Empty
        Private _ter_mcra_inactive As System.String = String.Empty
        Private _pri_mcrb_inactive As System.String = String.Empty
        Private _sec_mcrb_inactive As System.String = String.Empty
        Private _ter_mcrb_inactive As System.String = String.Empty
        Private _pri_payor_plan_number As System.String = String.Empty
        Private _sec_payor_plan_number As System.String = String.Empty
        Private _ter_payor_plan_number As System.String = String.Empty
        Private _pri_payor_dep_name_last As System.String = String.Empty
        Private _pri_payor_dep_name_first As System.String = String.Empty
        Private _pri_payor_dep_name_middle As System.String = String.Empty
        Private _pri_payor_dep_dob As System.String = String.Empty
        Private _pri_payor_dep_gender As System.String = String.Empty
        Private _sec_payor_dep_name_last As System.String = String.Empty
        Private _sec_payor_dep_name_first As System.String = String.Empty
        Private _sec_payor_dep_name_middle As System.String = String.Empty
        Private _sec_payor_dep_dob As System.String = String.Empty
        Private _sec_payor_dep_gender As System.String = String.Empty
        Private _ter_payor_dep_name_last As System.String = String.Empty
        Private _ter_payor_dep_name_first As System.String = String.Empty
        Private _ter_payor_dep_name_middle As System.String = String.Empty
        Private _ter_payor_dep_dob As System.String = String.Empty
        Private _ter_payor_dep_gender As System.String = String.Empty
        Private _pri_payor_res_plan_number As System.String = String.Empty
        Private _pri_payor_res_plan_name As System.String = String.Empty
        Private _pri_payor_res_group_number As System.String = String.Empty
        Private _pri_payor_res_group_name As System.String = String.Empty
        Private _pri_payor_hmo_plan_name As System.String = String.Empty
        Private _sec_payor_res_plan_number As System.String = String.Empty
        Private _sec_payor_res_plan_name As System.String = String.Empty
        Private _sec_payor_res_group_number As System.String = String.Empty
        Private _sec_payor_res_group_name As System.String = String.Empty
        Private _sec_payor_hmo_plan_name As System.String = String.Empty
        Private _ter_payor_res_plan_number As System.String = String.Empty
        Private _ter_payor_res_plan_name As System.String = String.Empty
        Private _ter_payor_res_group_number As System.String = String.Empty
        Private _ter_payor_res_group_name As System.String = String.Empty
        Private _ter_payor_hmo_plan_name As System.String = String.Empty
        Private _pri_payor_mcr_hmo As System.String = String.Empty
        Private _sec_payor_mcr_hmo As System.String = String.Empty
        Private _ter_payor_mcr_hmo As System.String = String.Empty
        Private _pri_payor_mcd_hmo As System.String = String.Empty
        Private _sec_payor_mcd_hmo As System.String = String.Empty
        Private _ter_payor_mcd_hmo As System.String = String.Empty
        Private _pri_payor_msp_flag As System.String = String.Empty
        Private _sec_payor_msp_flag As System.String = String.Empty
        Private _ter_payor_msp_flag As System.String = String.Empty
        Private _pri_plan_sponser As System.String = String.Empty
        Private _sec_plan_sponser As System.String = String.Empty
        Private _ter_plan_sponser As System.String = String.Empty
        Private _pri_other_payor As System.String = String.Empty
        Private _sec_other_payor As System.String = String.Empty
        Private _ter_other_payor As System.String = String.Empty
        Private _pri_payor_mcr_hospice As System.String = String.Empty
        Private _pri_payor_mcr_hospice_date As System.String = String.Empty
        Private _sec_payor_mcr_hospice As System.String = String.Empty
        Private _sec_payor_mcr_hospice_date As System.String = String.Empty
        Private _ter_payor_mcr_hospice As System.String = String.Empty
        Private _ter_payor_mcr_hospice_date As System.String = String.Empty
        Private _pri_payor_pcp_name As System.String = String.Empty
        Private _sec_payor_pcp_name As System.String = String.Empty
        Private _ter_payor_pcp_name As System.String = String.Empty


        ''' <summary>
        ''' Main Call to run rules 
        ''' </summary> 
        ''' <param name="dr">The DataRow that has tank </param> 
        ''' <returns></returns> 
        ''' <remarks></remarks> 
        Public Function ParseRuleDataRow() As Integer




            Dim r As Integer = 0










            'Dim collection As DataTableCollection = ds.Tables
            'For i As Integer = 0 To collection.Count - 1
            'Dim table As DataTable = collection(i)


            Try
                _patient_audit_trail_id = Convert.ToInt32(_dr("patient_audit_trail_id"))
            Catch ex As Exception
                _Err = _Err + "patient_audit_trail_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_id = Convert.ToInt32(_dr("patient_id"))
            Catch ex As Exception
                _Err = _Err + "patient_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_number = Convert.ToString(_dr("patient_number"))
            Catch ex As Exception
                _Err = _Err + "patient_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _med_record_number = Convert.ToString(_dr("med_record_number"))
            Catch ex As Exception
                _Err = _Err + "med_record_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_last_name = Convert.ToString(_dr("patient_last_name"))
            Catch ex As Exception
                _Err = _Err + "patient_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_first_name = Convert.ToString(_dr("patient_first_name"))
            Catch ex As Exception
                _Err = _Err + "patient_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_middle_initial = Convert.ToString(_dr("patient_middle_initial"))
            Catch ex As Exception
                _Err = _Err + "patient_middle_initial" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_title = Convert.ToString(_dr("patient_title"))
            Catch ex As Exception
                _Err = _Err + "patient_title" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_mother_maiden_name = Convert.ToString(_dr("patient_mother_maiden_name"))
            Catch ex As Exception
                _Err = _Err + "patient_mother_maiden_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_birth_date = Convert.ToString(_dr("patient_birth_date"))
            Catch ex As Exception
                _Err = _Err + "patient_birth_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_sex = Convert.ToString(_dr("patient_sex"))
            Catch ex As Exception
                _Err = _Err + "patient_sex" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_alias = Convert.ToString(_dr("patient_alias"))
            Catch ex As Exception
                _Err = _Err + "patient_alias" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_street_address = Convert.ToString(_dr("patient_street_address"))
            Catch ex As Exception
                _Err = _Err + "patient_street_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_address2 = Convert.ToString(_dr("patient_address2"))
            Catch ex As Exception
                _Err = _Err + "patient_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_city = Convert.ToString(_dr("patient_city"))
            Catch ex As Exception
                _Err = _Err + "patient_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_state = Convert.ToString(_dr("patient_state"))
            Catch ex As Exception
                _Err = _Err + "patient_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_zip_code = Convert.ToString(_dr("patient_zip_code"))
            Catch ex As Exception
                _Err = _Err + "patient_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_country_code = Convert.ToString(_dr("patient_country_code"))
            Catch ex As Exception
                _Err = _Err + "patient_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_home_phone_number = Convert.ToString(_dr("patient_home_phone_number"))
            Catch ex As Exception
                _Err = _Err + "patient_home_phone_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_email = Convert.ToString(_dr("patient_email"))
            Catch ex As Exception
                _Err = _Err + "patient_email" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_language = Convert.ToString(_dr("patient_language"))
            Catch ex As Exception
                _Err = _Err + "patient_language" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_marital_status = Convert.ToString(_dr("patient_marital_status"))
            Catch ex As Exception
                _Err = _Err + "patient_marital_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_religion_code = Convert.ToString(_dr("patient_religion_code"))
            Catch ex As Exception
                _Err = _Err + "patient_religion_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_ssn = Convert.ToString(_dr("patient_ssn"))
            Catch ex As Exception
                _Err = _Err + "patient_ssn" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_class = Convert.ToString(_dr("patient_class"))
            Catch ex As Exception
                _Err = _Err + "patient_class" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_location = Convert.ToString(_dr("patient_location"))
            Catch ex As Exception
                _Err = _Err + "patient_location" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admit_type = Convert.ToString(_dr("admit_type"))
            Catch ex As Exception
                _Err = _Err + "admit_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _attending_doctor_number = Convert.ToString(_dr("attending_doctor_number"))
            Catch ex As Exception
                _Err = _Err + "attending_doctor_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _attending_doctor_name = Convert.ToString(_dr("attending_doctor_name"))
            Catch ex As Exception
                _Err = _Err + "attending_doctor_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_number = Convert.ToString(_dr("referring_doctor_number"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_name = Convert.ToString(_dr("referring_doctor_name"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _consulting_doctor_number = Convert.ToString(_dr("consulting_doctor_number"))
            Catch ex As Exception
                _Err = _Err + "consulting_doctor_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _consulting_doctor_name = Convert.ToString(_dr("consulting_doctor_name"))
            Catch ex As Exception
                _Err = _Err + "consulting_doctor_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _hospital_service_code = Convert.ToString(_dr("hospital_service_code"))
            Catch ex As Exception
                _Err = _Err + "hospital_service_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_temp_location = Convert.ToString(_dr("patient_temp_location"))
            Catch ex As Exception
                _Err = _Err + "patient_temp_location" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admit_source = Convert.ToString(_dr("admit_source"))
            Catch ex As Exception
                _Err = _Err + "admit_source" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admitting_doctor_number = Convert.ToString(_dr("admitting_doctor_number"))
            Catch ex As Exception
                _Err = _Err + "admitting_doctor_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admitting_doctor_name = Convert.ToString(_dr("admitting_doctor_name"))
            Catch ex As Exception
                _Err = _Err + "admitting_doctor_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _account_type = Convert.ToString(_dr("account_type"))
            Catch ex As Exception
                _Err = _Err + "account_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _account_class = Convert.ToString(_dr("account_class"))
            Catch ex As Exception
                _Err = _Err + "account_class" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _servicing_facility = Convert.ToString(_dr("servicing_facility"))
            Catch ex As Exception
                _Err = _Err + "servicing_facility" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admitting_date = Convert.ToString(_dr("admitting_date"))
            Catch ex As Exception
                _Err = _Err + "admitting_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _discharge_date = Convert.ToString(_dr("discharge_date"))
            Catch ex As Exception
                _Err = _Err + "discharge_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diagnosis_code = Convert.ToString(_dr("diagnosis_code"))
            Catch ex As Exception
                _Err = _Err + "diagnosis_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diagnosis_desc = Convert.ToString(_dr("diagnosis_desc"))
            Catch ex As Exception
                _Err = _Err + "diagnosis_desc" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _drg_code = Convert.ToString(_dr("drg_code"))
            Catch ex As Exception
                _Err = _Err + "drg_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _accident_date_time = Convert.ToString(_dr("accident_date_time"))
            Catch ex As Exception
                _Err = _Err + "accident_date_time" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _accident_code = Convert.ToString(_dr("accident_code"))
            Catch ex As Exception
                _Err = _Err + "accident_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _occurrence_codes = Convert.ToString(_dr("occurrence_codes"))
            Catch ex As Exception
                _Err = _Err + "occurrence_codes" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _occurrence_code_dates = Convert.ToString(_dr("occurrence_code_dates"))
            Catch ex As Exception
                _Err = _Err + "occurrence_code_dates" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_last_name = Convert.ToString(_dr("guarantor_last_name"))
            Catch ex As Exception
                _Err = _Err + "guarantor_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_first_name = Convert.ToString(_dr("guarantor_first_name"))
            Catch ex As Exception
                _Err = _Err + "guarantor_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_middle_initial = Convert.ToString(_dr("gurantor_middle_initial"))
            Catch ex As Exception
                _Err = _Err + "gurantor_middle_initial" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_spouse_name = Convert.ToString(_dr("guarantor_spouse_name"))
            Catch ex As Exception
                _Err = _Err + "guarantor_spouse_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_street_address = Convert.ToString(_dr("gurantor_street_address"))
            Catch ex As Exception
                _Err = _Err + "gurantor_street_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_address2 = Convert.ToString(_dr("gurantor_address2"))
            Catch ex As Exception
                _Err = _Err + "gurantor_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_city = Convert.ToString(_dr("gurantor_city"))
            Catch ex As Exception
                _Err = _Err + "gurantor_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_state = Convert.ToString(_dr("gurantor_state"))
            Catch ex As Exception
                _Err = _Err + "gurantor_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_zip_code = Convert.ToString(_dr("gurantor_zip_code"))
            Catch ex As Exception
                _Err = _Err + "gurantor_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_country_code = Convert.ToString(_dr("guarantor_country_code"))
            Catch ex As Exception
                _Err = _Err + "guarantor_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_home_phone_num = Convert.ToString(_dr("guarantor_home_phone_num"))
            Catch ex As Exception
                _Err = _Err + "guarantor_home_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_email = Convert.ToString(_dr("guarantor_email"))
            Catch ex As Exception
                _Err = _Err + "guarantor_email" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_dob = Convert.ToString(_dr("guarantor_dob"))
            Catch ex As Exception
                _Err = _Err + "guarantor_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_dob = Convert.ToString(_dr("gurantor_dob"))
            Catch ex As Exception
                _Err = _Err + "gurantor_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_sex = Convert.ToString(_dr("guarantor_sex"))
            Catch ex As Exception
                _Err = _Err + "guarantor_sex" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_type = Convert.ToString(_dr("guarantor_type"))
            Catch ex As Exception
                _Err = _Err + "guarantor_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_relationship = Convert.ToString(_dr("guarantor_relationship"))
            Catch ex As Exception
                _Err = _Err + "guarantor_relationship" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_ssn = Convert.ToString(_dr("gurantor_ssn"))
            Catch ex As Exception
                _Err = _Err + "gurantor_ssn" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_name = Convert.ToString(_dr("gurantor_emp_name"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_address = Convert.ToString(_dr("gurantor_emp_address"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_address2 = Convert.ToString(_dr("gurantor_emp_address2"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_city = Convert.ToString(_dr("gurantor_emp_city"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_state = Convert.ToString(_dr("gurantor_emp_state"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_zip_code = Convert.ToString(_dr("gurantor_emp_zip_code"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_phone_number = Convert.ToString(_dr("gurantor_emp_phone_number"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_phone_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_phone_extension = Convert.ToString(_dr("gurantor_emp_phone_extension"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_phone_extension" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_emp_country_code = Convert.ToString(_dr("gurantor_emp_country_code"))
            Catch ex As Exception
                _Err = _Err + "gurantor_emp_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_last_name = Convert.ToString(_dr("contact_last_name"))
            Catch ex As Exception
                _Err = _Err + "contact_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_first_name = Convert.ToString(_dr("contact_first_name"))
            Catch ex As Exception
                _Err = _Err + "contact_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_middle_initial = Convert.ToString(_dr("contact_middle_initial"))
            Catch ex As Exception
                _Err = _Err + "contact_middle_initial" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_relationship_to_pat = Convert.ToString(_dr("contact_relationship_to_pat"))
            Catch ex As Exception
                _Err = _Err + "contact_relationship_to_pat" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_street_address = Convert.ToString(_dr("contact_street_address"))
            Catch ex As Exception
                _Err = _Err + "contact_street_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_address2 = Convert.ToString(_dr("contact_address2"))
            Catch ex As Exception
                _Err = _Err + "contact_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_city = Convert.ToString(_dr("contact_city"))
            Catch ex As Exception
                _Err = _Err + "contact_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_state = Convert.ToString(_dr("contact_state"))
            Catch ex As Exception
                _Err = _Err + "contact_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_name = Convert.ToString(_dr("nearest_relative_name"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_phone = Convert.ToString(_dr("nearest_relative_phone"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_phone" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_rel = Convert.ToString(_dr("nearest_relative_rel"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_rel" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_zip_code = Convert.ToString(_dr("contact_zip_code"))
            Catch ex As Exception
                _Err = _Err + "contact_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_country_code = Convert.ToString(_dr("contact_country_code"))
            Catch ex As Exception
                _Err = _Err + "contact_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_home_phone_num = Convert.ToString(_dr("contact_home_phone_num"))
            Catch ex As Exception
                _Err = _Err + "contact_home_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_role = Convert.ToString(_dr("contact_role"))
            Catch ex As Exception
                _Err = _Err + "contact_role" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_number = Convert.ToString(_dr("pri_ins_number"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_name = Convert.ToString(_dr("pri_ins_name"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_address = Convert.ToString(_dr("pri_ins_address"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_address_2 = Convert.ToString(_dr("pri_address_2"))
            Catch ex As Exception
                _Err = _Err + "pri_address_2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_city = Convert.ToString(_dr("pri_ins_city"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_state = Convert.ToString(_dr("pri_ins_state"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_zip_code = Convert.ToString(_dr("pri_ins_zip_code"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_country_code = Convert.ToString(_dr("pri_ins_country_code"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_contact_name = Convert.ToString(_dr("pri_ins_contact_name"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_contact_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_phone_number = Convert.ToString(_dr("pri_ins_phone_number"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_phone_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_group_name = Convert.ToString(_dr("pri_ins_group_name"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_group_number = Convert.ToString(_dr("pri_ins_group_number"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_group_emp_id = Convert.ToString(_dr("pri_insured_group_emp_id"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_group_emp_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_group_emp_name = Convert.ToString(_dr("pri_insured_group_emp_name"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_group_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_plan_effective_date = Convert.ToString(_dr("pri_ins_plan_effective_date"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_plan_effective_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_plan_expiration_date = Convert.ToString(_dr("pri_ins_plan_expiration_date"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_plan_expiration_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_plan_type = Convert.ToString(_dr("pri_ins_plan_type"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_last_name = Convert.ToString(_dr("pri_insured_last_name"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_first_name = Convert.ToString(_dr("pri_insured_first_name"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_relationship = Convert.ToString(_dr("pri_insured_relationship"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_relationship" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_dob = Convert.ToString(_dr("pri_insured_dob"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _insured_dob = Convert.ToString(_dr("insured_dob"))
            Catch ex As Exception
                _Err = _Err + "insured_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_address = Convert.ToString(_dr("pri_insured_address"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_address2 = Convert.ToString(_dr("pri_insured_address2"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_city = Convert.ToString(_dr("pri_insured_city"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_state = Convert.ToString(_dr("pri_insured_state"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_zip = Convert.ToString(_dr("pri_insured_zip"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_zip" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_cob_priority = Convert.ToString(_dr("pri_cob_priority"))
            Catch ex As Exception
                _Err = _Err + "pri_cob_priority" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_date_of_verification = Convert.ToString(_dr("pri_date_of_verification"))
            Catch ex As Exception
                _Err = _Err + "pri_date_of_verification" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_name_of_verifier = Convert.ToString(_dr("pri_name_of_verifier"))
            Catch ex As Exception
                _Err = _Err + "pri_name_of_verifier" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_policy_number = Convert.ToString(_dr("pri_ins_policy_number"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_policy_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_sex = Convert.ToString(_dr("pri_insured_sex"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_sex" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_address = Convert.ToString(_dr("pri_insured_emp_address"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_address2 = Convert.ToString(_dr("pri_insured_emp_address2"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_city = Convert.ToString(_dr("pri_insured_emp_city"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_state = Convert.ToString(_dr("pri_insured_emp_state"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_zip_code = Convert.ToString(_dr("pri_insured_emp_zip_code"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_verification_status = Convert.ToString(_dr("pri_ins_verification_status"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_verification_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_employee_id = Convert.ToString(_dr("pri_insured_employee_id"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_employee_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_ssn = Convert.ToString(_dr("pri_insured_ssn"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_ssn" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_name = Convert.ToString(_dr("pri_insured_emp_name"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_coverage_num = Convert.ToString(_dr("pri_ins_coverage_num"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_coverage_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_plan_number = Convert.ToString(_dr("sec_plan_number"))
            Catch ex As Exception
                _Err = _Err + "sec_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_number = Convert.ToString(_dr("sec_ins_number"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_name = Convert.ToString(_dr("sec_ins_name"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_address = Convert.ToString(_dr("sec_ins_address"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_address_2 = Convert.ToString(_dr("sec_address_2"))
            Catch ex As Exception
                _Err = _Err + "sec_address_2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_city = Convert.ToString(_dr("sec_ins_city"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_state = Convert.ToString(_dr("sec_ins_state"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_zip_code = Convert.ToString(_dr("sec_ins_zip_code"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_country_code = Convert.ToString(_dr("sec_ins_country_code"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_contact_name = Convert.ToString(_dr("sec_ins_contact_name"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_contact_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_phone_number = Convert.ToString(_dr("sec_ins_phone_number"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_phone_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_group_name = Convert.ToString(_dr("sec_ins_group_name"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_group_number = Convert.ToString(_dr("sec_ins_group_number"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_group_emp_id = Convert.ToString(_dr("sec_insured_group_emp_id"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_group_emp_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_group_emp_name = Convert.ToString(_dr("sec_insured_group_emp_name"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_group_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_plan_effective_date = Convert.ToString(_dr("sec_ins_plan_effective_date"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_plan_effective_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_plan_expiration_date = Convert.ToString(_dr("sec_ins_plan_expiration_date"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_plan_expiration_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_plan_type = Convert.ToString(_dr("sec_ins_plan_type"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_last_name = Convert.ToString(_dr("sec_insured_last_name"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_first_name = Convert.ToString(_dr("sec_insured_first_name"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_relationship = Convert.ToString(_dr("sec_insured_relationship"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_relationship" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_dob = Convert.ToString(_dr("sec_insured_dob"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_address = Convert.ToString(_dr("sec_insured_address"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_address2 = Convert.ToString(_dr("sec_insured_address2"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_city = Convert.ToString(_dr("sec_insured_city"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_state = Convert.ToString(_dr("sec_insured_state"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_zip = Convert.ToString(_dr("sec_insured_zip"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_zip" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_cob_priority = Convert.ToString(_dr("sec_cob_priority"))
            Catch ex As Exception
                _Err = _Err + "sec_cob_priority" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_date_of_verification = Convert.ToString(_dr("sec_date_of_verification"))
            Catch ex As Exception
                _Err = _Err + "sec_date_of_verification" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_name_of_verifier = Convert.ToString(_dr("sec_name_of_verifier"))
            Catch ex As Exception
                _Err = _Err + "sec_name_of_verifier" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_policy_number = Convert.ToString(_dr("sec_ins_policy_number"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_policy_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_sex = Convert.ToString(_dr("sec_insured_sex"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_sex" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_address = Convert.ToString(_dr("sec_insured_emp_address"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_address2 = Convert.ToString(_dr("sec_insured_emp_address2"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_city = Convert.ToString(_dr("sec_insured_emp_city"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_state = Convert.ToString(_dr("sec_insured_emp_state"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_zip_code = Convert.ToString(_dr("sec_insured_emp_zip_code"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_verification_status = Convert.ToString(_dr("sec_ins_verification_status"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_verification_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_employee_id = Convert.ToString(_dr("sec_insured_employee_id"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_employee_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_ssn = Convert.ToString(_dr("sec_insured_ssn"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_ssn" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_employer_name = Convert.ToString(_dr("sec_insured_employer_name"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_employer_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_plan_number = Convert.ToString(_dr("ter_plan_number"))
            Catch ex As Exception
                _Err = _Err + "ter_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_number = Convert.ToString(_dr("ter_ins_number"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_name = Convert.ToString(_dr("ter_ins_name"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_address = Convert.ToString(_dr("ter_ins_address"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_address_2 = Convert.ToString(_dr("ter_address_2"))
            Catch ex As Exception
                _Err = _Err + "ter_address_2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_city = Convert.ToString(_dr("ter_ins_city"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_state = Convert.ToString(_dr("ter_ins_state"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_zip_code = Convert.ToString(_dr("ter_ins_zip_code"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_country_code = Convert.ToString(_dr("ter_ins_country_code"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_contact_name = Convert.ToString(_dr("ter_ins_contact_name"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_contact_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_phone_number = Convert.ToString(_dr("ter_ins_phone_number"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_phone_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_group_name = Convert.ToString(_dr("ter_ins_group_name"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_group_number = Convert.ToString(_dr("ter_ins_group_number"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_group_emp_id = Convert.ToString(_dr("ter_insured_group_emp_id"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_group_emp_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_group_emp_name = Convert.ToString(_dr("ter_insured_group_emp_name"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_group_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_plan_effective_date = Convert.ToString(_dr("ter_ins_plan_effective_date"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_plan_effective_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_plan_expiration_date = Convert.ToString(_dr("ter_ins_plan_expiration_date"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_plan_expiration_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_plan_type = Convert.ToString(_dr("ter_ins_plan_type"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_last_name = Convert.ToString(_dr("ter_insured_last_name"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_first_name = Convert.ToString(_dr("ter_insured_first_name"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_relationship = Convert.ToString(_dr("ter_insured_relationship"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_relationship" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_dob = Convert.ToString(_dr("ter_insured_dob"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_address = Convert.ToString(_dr("ter_insured_address"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_address2 = Convert.ToString(_dr("ter_insured_address2"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_city = Convert.ToString(_dr("ter_insured_city"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_state = Convert.ToString(_dr("ter_insured_state"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_zip = Convert.ToString(_dr("ter_insured_zip"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_zip" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_cob_priority = Convert.ToString(_dr("ter_cob_priority"))
            Catch ex As Exception
                _Err = _Err + "ter_cob_priority" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_date_of_verification = Convert.ToString(_dr("ter_date_of_verification"))
            Catch ex As Exception
                _Err = _Err + "ter_date_of_verification" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_name_of_verifier = Convert.ToString(_dr("ter_name_of_verifier"))
            Catch ex As Exception
                _Err = _Err + "ter_name_of_verifier" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_policy_number = Convert.ToString(_dr("ter_ins_policy_number"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_policy_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_sex = Convert.ToString(_dr("ter_insured_sex"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_sex" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_address = Convert.ToString(_dr("ter_insured_emp_address"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_address2 = Convert.ToString(_dr("ter_insured_emp_address2"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_city = Convert.ToString(_dr("ter_insured_emp_city"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_state = Convert.ToString(_dr("ter_insured_emp_state"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_zip_code = Convert.ToString(_dr("ter_insured_emp_zip_code"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_verification_status = Convert.ToString(_dr("ter_ins_verification_status"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_verification_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_employee_id = Convert.ToString(_dr("ter_insured_employee_id"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_employee_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_ssn = Convert.ToString(_dr("ter_insured_ssn"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_ssn" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_employer_name = Convert.ToString(_dr("ter_insured_employer_name"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_employer_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_plan_number = Convert.ToString(_dr("oth_plan_number"))
            Catch ex As Exception
                _Err = _Err + "oth_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_name = Convert.ToString(_dr("oth_name"))
            Catch ex As Exception
                _Err = _Err + "oth_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_address = Convert.ToString(_dr("oth_address"))
            Catch ex As Exception
                _Err = _Err + "oth_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_address_2 = Convert.ToString(_dr("oth_address_2"))
            Catch ex As Exception
                _Err = _Err + "oth_address_2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_city = Convert.ToString(_dr("oth_ins_city"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_state = Convert.ToString(_dr("oth_ins_state"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_zip_code = Convert.ToString(_dr("oth_ins_zip_code"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_country_code = Convert.ToString(_dr("oth_ins_country_code"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_contact_name = Convert.ToString(_dr("oth_ins_contact_name"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_contact_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_phone_number = Convert.ToString(_dr("oth_ins_phone_number"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_phone_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_group_name = Convert.ToString(_dr("oth_group_name"))
            Catch ex As Exception
                _Err = _Err + "oth_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_group_number = Convert.ToString(_dr("oth_group_number"))
            Catch ex As Exception
                _Err = _Err + "oth_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_group_emp_id = Convert.ToString(_dr("oth_insured_group_emp_id"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_group_emp_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_group_emp_name = Convert.ToString(_dr("oth_insured_group_emp_name"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_group_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_plan_effective_date = Convert.ToString(_dr("oth_ins_plan_effective_date"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_plan_effective_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_plan_expiration_date = Convert.ToString(_dr("oth_ins_plan_expiration_date"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_plan_expiration_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_plan_type = Convert.ToString(_dr("oth_ins_plan_type"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_last_name = Convert.ToString(_dr("oth_insured_last_name"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_last_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_first_name = Convert.ToString(_dr("oth_insured_first_name"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_first_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_relationship = Convert.ToString(_dr("oth_insured_relationship"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_relationship" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_dob = Convert.ToString(_dr("oth_insured_dob"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_address = Convert.ToString(_dr("oth_insured_address"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_address2 = Convert.ToString(_dr("oth_insured_address2"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_city = Convert.ToString(_dr("oth_insured_city"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_state = Convert.ToString(_dr("oth_insured_state"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_zip = Convert.ToString(_dr("oth_insured_zip"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_zip" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_cob_priority = Convert.ToString(_dr("oth_cob_priority"))
            Catch ex As Exception
                _Err = _Err + "oth_cob_priority" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_date_of_verification = Convert.ToString(_dr("oth_date_of_verification"))
            Catch ex As Exception
                _Err = _Err + "oth_date_of_verification" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_name_of_verifier = Convert.ToString(_dr("oth_name_of_verifier"))
            Catch ex As Exception
                _Err = _Err + "oth_name_of_verifier" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_policy_number = Convert.ToString(_dr("oth_ins_policy_number"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_policy_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_sex = Convert.ToString(_dr("oth_insured_sex"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_sex" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_address = Convert.ToString(_dr("oth_insured_emp_address"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_address2 = Convert.ToString(_dr("oth_insured_emp_address2"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_city = Convert.ToString(_dr("oth_insured_emp_city"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_state = Convert.ToString(_dr("oth_insured_emp_state"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_zip_code = Convert.ToString(_dr("oth_insured_emp_zip_code"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_verification_status = Convert.ToString(_dr("oth_ins_verification_status"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_verification_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_employee_id = Convert.ToString(_dr("oth_insured_employee_id"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_employee_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_ssn = Convert.ToString(_dr("oth_insured_ssn"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_ssn" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_employer_name = Convert.ToString(_dr("oth_insured_employer_name"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_employer_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _client_facility_code = Convert.ToString(_dr("client_facility_code"))
            Catch ex As Exception
                _Err = _Err + "client_facility_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _create_date = Convert.ToString(_dr("create_date"))
            Catch ex As Exception
                _Err = _Err + "create_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _create_time = Convert.ToString(_dr("create_time"))
            Catch ex As Exception
                _Err = _Err + "create_time" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _source_file = Convert.ToString(_dr("source_file"))
            Catch ex As Exception
                _Err = _Err + "source_file" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _system_type = Convert.ToString(_dr("system_type"))
            Catch ex As Exception
                _Err = _Err + "system_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sub_system = Convert.ToString(_dr("sub_system"))
            Catch ex As Exception
                _Err = _Err + "sub_system" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _update_flag = Convert.ToInt32(_dr("update_flag"))
            Catch ex As Exception
                _Err = _Err + "update_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ctvision_pt = Convert.ToString(_dr("ctvision_pt"))
            Catch ex As Exception
                _Err = _Err + "ctvision_pt" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ctvision_fc = Convert.ToString(_dr("ctvision_fc"))
            Catch ex As Exception
                _Err = _Err + "ctvision_fc" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _id = Convert.ToDecimal(_dr("id"))
            Catch ex As Exception
                _Err = _Err + "id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _event_type = Convert.ToString(_dr("event_type"))
            Catch ex As Exception
                _Err = _Err + "event_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _event_detail = Convert.ToString(_dr("event_detail"))
            Catch ex As Exception
                _Err = _Err + "event_detail" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pat_hosp_code = Convert.ToString(_dr("pat_hosp_code"))
            Catch ex As Exception
                _Err = _Err + "pat_hosp_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _operator_id = Convert.ToString(_dr("operator_id"))
            Catch ex As Exception
                _Err = _Err + "operator_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _workstation_id = Convert.ToString(_dr("workstation_id"))
            Catch ex As Exception
                _Err = _Err + "workstation_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _event_datetime = Convert.ToString(_dr("event_datetime"))
            Catch ex As Exception
                _Err = _Err + "event_datetime" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _message_datetime = Convert.ToString(_dr("message_datetime"))
            Catch ex As Exception
                _Err = _Err + "message_datetime" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _total_charge_amount = Convert.ToString(_dr("total_charge_amount"))
            Catch ex As Exception
                _Err = _Err + "total_charge_amount" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _audit_flag = Convert.ToString(_dr("audit_flag"))
            Catch ex As Exception
                _Err = _Err + "audit_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pid = Convert.ToInt32(_dr("pid"))
            Catch ex As Exception
                _Err = _Err + "pid" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_work_phone_num = Convert.ToString(_dr("patient_work_phone_num"))
            Catch ex As Exception
                _Err = _Err + "patient_work_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_other_phone_num = Convert.ToString(_dr("patient_other_phone_num"))
            Catch ex As Exception
                _Err = _Err + "patient_other_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_status = Convert.ToString(_dr("patient_status"))
            Catch ex As Exception
                _Err = _Err + "patient_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_pre_reg = Convert.ToString(_dr("patient_pre_reg"))
            Catch ex As Exception
                _Err = _Err + "patient_pre_reg" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_code = Convert.ToString(_dr("patient_emp_code"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_name = Convert.ToString(_dr("patient_emp_name"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_status = Convert.ToString(_dr("patient_emp_status"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_phone_num = Convert.ToString(_dr("patient_emp_phone_num"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_work_phone_num = Convert.ToString(_dr("contact_work_phone_num"))
            Catch ex As Exception
                _Err = _Err + "contact_work_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _contact_other_phone_num = Convert.ToString(_dr("contact_other_phone_num"))
            Catch ex As Exception
                _Err = _Err + "contact_other_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_other_phone_num = Convert.ToString(_dr("guarantor_other_phone_num"))
            Catch ex As Exception
                _Err = _Err + "guarantor_other_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_occupation = Convert.ToString(_dr("guarantor_occupation"))
            Catch ex As Exception
                _Err = _Err + "guarantor_occupation" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_emp_code = Convert.ToString(_dr("guarantor_emp_code"))
            Catch ex As Exception
                _Err = _Err + "guarantor_emp_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nebo_patient_id = Convert.ToString(_dr("nebo_patient_id"))
            Catch ex As Exception
                _Err = _Err + "nebo_patient_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nebo_guarantor_id = Convert.ToString(_dr("nebo_guarantor_id"))
            Catch ex As Exception
                _Err = _Err + "nebo_guarantor_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_treat_auth_id = Convert.ToString(_dr("pri_ins_treat_auth_id"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_treat_auth_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_bc_plan_code = Convert.ToString(_dr("pri_ins_bc_plan_code"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_bc_plan_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_bc_plan_code = Convert.ToString(_dr("sec_ins_bc_plan_code"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_bc_plan_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_bc_plan_code = Convert.ToString(_dr("ter_ins_bc_plan_code"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_bc_plan_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_home_phone_num = Convert.ToString(_dr("pri_insured_home_phone_num"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_home_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_other_phone_num = Convert.ToString(_dr("pri_insured_other_phone_num"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_other_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_phone_num = Convert.ToString(_dr("pri_insured_emp_phone_num"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_treat_auth_id = Convert.ToString(_dr("sec_ins_treat_auth_id"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_treat_auth_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_home_phone_num = Convert.ToString(_dr("sec_insured_home_phone_num"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_home_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_other_phone_num = Convert.ToString(_dr("sec_insured_other_phone_num"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_other_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_phone_num = Convert.ToString(_dr("sec_insured_emp_phone_num"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_treat_auth_id = Convert.ToString(_dr("ter_ins_treat_auth_id"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_treat_auth_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_home_phone_num = Convert.ToString(_dr("ter_insured_home_phone_num"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_home_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_other_phone_num = Convert.ToString(_dr("ter_insured_other_phone_num"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_other_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_phone_num = Convert.ToString(_dr("ter_insured_emp_phone_num"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_treat_auth_id = Convert.ToString(_dr("oth_ins_treat_auth_id"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_treat_auth_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_home_phone_num = Convert.ToString(_dr("oth_insured_home_phone_num"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_home_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_nebo_field = Convert.ToString(_dr("pri_nebo_field"))
            Catch ex As Exception
                _Err = _Err + "pri_nebo_field" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_nebo_field = Convert.ToString(_dr("sec_nebo_field"))
            Catch ex As Exception
                _Err = _Err + "sec_nebo_field" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_nebo_field = Convert.ToString(_dr("ter_nebo_field"))
            Catch ex As Exception
                _Err = _Err + "ter_nebo_field" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_nebo_field = Convert.ToString(_dr("oth_nebo_field"))
            Catch ex As Exception
                _Err = _Err + "oth_nebo_field" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _name_aka = Convert.ToString(_dr("name_aka"))
            Catch ex As Exception
                _Err = _Err + "name_aka" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _special_guest = Convert.ToString(_dr("special_guest"))
            Catch ex As Exception
                _Err = _Err + "special_guest" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _day_phone_num = Convert.ToString(_dr("day_phone_num"))
            Catch ex As Exception
                _Err = _Err + "day_phone_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_care_physician = Convert.ToString(_dr("primary_care_physician"))
            Catch ex As Exception
                _Err = _Err + "primary_care_physician" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_physician = Convert.ToString(_dr("referring_physician"))
            Catch ex As Exception
                _Err = _Err + "referring_physician" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _religion = Convert.ToString(_dr("religion"))
            Catch ex As Exception
                _Err = _Err + "religion" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _race = Convert.ToString(_dr("race"))
            Catch ex As Exception
                _Err = _Err + "race" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _advanced_directive = Convert.ToString(_dr("advanced_directive"))
            Catch ex As Exception
                _Err = _Err + "advanced_directive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _smoking_education = Convert.ToString(_dr("smoking_education"))
            Catch ex As Exception
                _Err = _Err + "smoking_education" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _msp_form_detail = Convert.ToString(_dr("msp_form_detail"))
            Catch ex As Exception
                _Err = _Err + "msp_form_detail" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _medicare_plan = Convert.ToString(_dr("medicare_plan"))
            Catch ex As Exception
                _Err = _Err + "medicare_plan" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ppo_hmo = Convert.ToString(_dr("ppo_hmo"))
            Catch ex As Exception
                _Err = _Err + "ppo_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _workers_comp_detail = Convert.ToString(_dr("workers_comp_detail"))
            Catch ex As Exception
                _Err = _Err + "workers_comp_detail" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mva = Convert.ToString(_dr("mva"))
            Catch ex As Exception
                _Err = _Err + "mva" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _chief_complaint = Convert.ToString(_dr("chief_complaint"))
            Catch ex As Exception
                _Err = _Err + "chief_complaint" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _charity_care = Convert.ToString(_dr("charity_care"))
            Catch ex As Exception
                _Err = _Err + "charity_care" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _plan_expiration = Convert.ToString(_dr("plan_expiration"))
            Catch ex As Exception
                _Err = _Err + "plan_expiration" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referral_num = Convert.ToString(_dr("referral_num"))
            Catch ex As Exception
                _Err = _Err + "referral_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_sub_addres = Convert.ToString(_dr("primary_sub_addres"))
            Catch ex As Exception
                _Err = _Err + "primary_sub_addres" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _recipient_num = Convert.ToString(_dr("recipient_num"))
            Catch ex As Exception
                _Err = _Err + "recipient_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ins_mail_claim = Convert.ToString(_dr("ins_mail_claim"))
            Catch ex As Exception
                _Err = _Err + "ins_mail_claim" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_alert = Convert.ToString(_dr("patient_alert"))
            Catch ex As Exception
                _Err = _Err + "patient_alert" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _authorization_num = Convert.ToString(_dr("authorization_num"))
            Catch ex As Exception
                _Err = _Err + "authorization_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _copay = Convert.ToString(_dr("copay"))
            Catch ex As Exception
                _Err = _Err + "copay" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ins_verification = Convert.ToString(_dr("ins_verification"))
            Catch ex As Exception
                _Err = _Err + "ins_verification" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ins_comp_name = Convert.ToString(_dr("ins_comp_name"))
            Catch ex As Exception
                _Err = _Err + "ins_comp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _region_code = Convert.ToString(_dr("region_code"))
            Catch ex As Exception
                _Err = _Err + "region_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _division_code = Convert.ToString(_dr("division_code"))
            Catch ex As Exception
                _Err = _Err + "division_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _facility_code = Convert.ToString(_dr("facility_code"))
            Catch ex As Exception
                _Err = _Err + "facility_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _msp_error = Convert.ToString(_dr("msp_error"))
            Catch ex As Exception
                _Err = _Err + "msp_error" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_type = Convert.ToString(_dr("patient_type"))
            Catch ex As Exception
                _Err = _Err + "patient_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _npp = Convert.ToString(_dr("npp"))
            Catch ex As Exception
                _Err = _Err + "npp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _npp_date = Convert.ToString(_dr("npp_date"))
            Catch ex As Exception
                _Err = _Err + "npp_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _orgon_donor = Convert.ToString(_dr("orgon_donor"))
            Catch ex As Exception
                _Err = _Err + "orgon_donor" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _gurantor_street_address2 = Convert.ToString(_dr("gurantor_street_address2"))
            Catch ex As Exception
                _Err = _Err + "gurantor_street_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ad_date = Convert.ToString(_dr("ad_date"))
            Catch ex As Exception
                _Err = _Err + "ad_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _financial_class = Convert.ToString(_dr("financial_class"))
            Catch ex As Exception
                _Err = _Err + "financial_class" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _roi_consent = Convert.ToString(_dr("roi_consent"))
            Catch ex As Exception
                _Err = _Err + "roi_consent" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _staff_alert = Convert.ToString(_dr("staff_alert"))
            Catch ex As Exception
                _Err = _Err + "staff_alert" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mcd_days = Convert.ToString(_dr("mcd_days"))
            Catch ex As Exception
                _Err = _Err + "mcd_days" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _source = Convert.ToString(_dr("source"))
            Catch ex As Exception
                _Err = _Err + "source" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_type = Convert.ToString(_dr("pri_ins_type"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_type = Convert.ToString(_dr("sec_ins_type"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_type = Convert.ToString(_dr("ter_ins_type"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_type = Convert.ToString(_dr("oth_ins_type"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _medicare_scanned = Convert.ToString(_dr("medicare_scanned"))
            Catch ex As Exception
                _Err = _Err + "medicare_scanned" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _order_scanned = Convert.ToString(_dr("order_scanned"))
            Catch ex As Exception
                _Err = _Err + "order_scanned" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_status = Convert.ToString(_dr("pri_insured_emp_status"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_policy_type = Convert.ToString(_dr("pri_policy_type"))
            Catch ex As Exception
                _Err = _Err + "pri_policy_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_medicare_hic = Convert.ToString(_dr("pri_medicare_hic"))
            Catch ex As Exception
                _Err = _Err + "pri_medicare_hic" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_country_code = Convert.ToString(_dr("pri_insured_country_code"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_emp_country_code = Convert.ToString(_dr("pri_insured_emp_country_code"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_emp_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_status = Convert.ToString(_dr("sec_insured_emp_status"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_policy_type = Convert.ToString(_dr("sec_policy_type"))
            Catch ex As Exception
                _Err = _Err + "sec_policy_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_medicare_hic = Convert.ToString(_dr("sec_medicare_hic"))
            Catch ex As Exception
                _Err = _Err + "sec_medicare_hic" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_country_code = Convert.ToString(_dr("sec_insured_country_code"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_emp_country_code = Convert.ToString(_dr("sec_insured_emp_country_code"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_emp_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_status = Convert.ToString(_dr("ter_insured_emp_status"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_policy_type = Convert.ToString(_dr("ter_policy_type"))
            Catch ex As Exception
                _Err = _Err + "ter_policy_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_medicare_hic = Convert.ToString(_dr("ter_medicare_hic"))
            Catch ex As Exception
                _Err = _Err + "ter_medicare_hic" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_country_code = Convert.ToString(_dr("ter_insured_country_code"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_emp_country_code = Convert.ToString(_dr("ter_insured_emp_country_code"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_emp_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_status = Convert.ToString(_dr("oth_insured_emp_status"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_policy_type = Convert.ToString(_dr("oth_policy_type"))
            Catch ex As Exception
                _Err = _Err + "oth_policy_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_medicare_hic = Convert.ToString(_dr("oth_medicare_hic"))
            Catch ex As Exception
                _Err = _Err + "oth_medicare_hic" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_country_code = Convert.ToString(_dr("oth_insured_country_code"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_insured_emp_country_code = Convert.ToString(_dr("oth_insured_emp_country_code"))
            Catch ex As Exception
                _Err = _Err + "oth_insured_emp_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_ded = Convert.ToString(_dr("pri_ins_ded"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_ded" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_oopd = Convert.ToString(_dr("pri_ins_oopd"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_oopd" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_ded = Convert.ToString(_dr("sec_ins_ded"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_ded" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_oopd = Convert.ToString(_dr("sec_ins_oopd"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_oopd" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_ded = Convert.ToString(_dr("ter_ins_ded"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_ded" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_oopd = Convert.ToString(_dr("ter_ins_oopd"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_oopd" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_ded = Convert.ToString(_dr("oth_ins_ded"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_ded" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_race = Convert.ToString(_dr("patient_race"))
            Catch ex As Exception
                _Err = _Err + "patient_race" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_address = Convert.ToString(_dr("patient_emp_address"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_address2 = Convert.ToString(_dr("patient_emp_address2"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_city = Convert.ToString(_dr("patient_emp_city"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_state = Convert.ToString(_dr("patient_emp_state"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_emp_zip_code = Convert.ToString(_dr("patient_emp_zip_code"))
            Catch ex As Exception
                _Err = _Err + "patient_emp_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_us_citizenship = Convert.ToString(_dr("pri_insured_us_citizenship"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_us_citizenship" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_race = Convert.ToString(_dr("pri_insured_race"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_race" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_marital_status = Convert.ToString(_dr("pri_insured_marital_status"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_marital_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admission_priority = Convert.ToString(_dr("admission_priority"))
            Catch ex As Exception
                _Err = _Err + "admission_priority" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _family_doctor_name = Convert.ToString(_dr("family_doctor_name"))
            Catch ex As Exception
                _Err = _Err + "family_doctor_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _family_doctor_number = Convert.ToString(_dr("family_doctor_number"))
            Catch ex As Exception
                _Err = _Err + "family_doctor_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _from_date = Convert.ToString(_dr("from_date"))
            Catch ex As Exception
                _Err = _Err + "from_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _thru_date = Convert.ToString(_dr("thru_date"))
            Catch ex As Exception
                _Err = _Err + "thru_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _actual_los = Convert.ToString(_dr("actual_los"))
            Catch ex As Exception
                _Err = _Err + "actual_los" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _adt_comment = Convert.ToString(_dr("adt_comment"))
            Catch ex As Exception
                _Err = _Err + "adt_comment" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _arrived_by = Convert.ToString(_dr("arrived_by"))
            Catch ex As Exception
                _Err = _Err + "arrived_by" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diag2 = Convert.ToString(_dr("diag2"))
            Catch ex As Exception
                _Err = _Err + "diag2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diag3 = Convert.ToString(_dr("diag3"))
            Catch ex As Exception
                _Err = _Err + "diag3" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diag4 = Convert.ToString(_dr("diag4"))
            Catch ex As Exception
                _Err = _Err + "diag4" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diag5 = Convert.ToString(_dr("diag5"))
            Catch ex As Exception
                _Err = _Err + "diag5" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _diag6 = Convert.ToString(_dr("diag6"))
            Catch ex As Exception
                _Err = _Err + "diag6" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _last_hosp_from_date = Convert.ToString(_dr("last_hosp_from_date"))
            Catch ex As Exception
                _Err = _Err + "last_hosp_from_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _last_hosp_thru_date = Convert.ToString(_dr("last_hosp_thru_date"))
            Catch ex As Exception
                _Err = _Err + "last_hosp_thru_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _last_hosp_hospital = Convert.ToString(_dr("last_hosp_hospital"))
            Catch ex As Exception
                _Err = _Err + "last_hosp_hospital" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _room_location = Convert.ToString(_dr("room_location"))
            Catch ex As Exception
                _Err = _Err + "room_location" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _bed_location = Convert.ToString(_dr("bed_location"))
            Catch ex As Exception
                _Err = _Err + "bed_location" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _rooms_accom = Convert.ToString(_dr("rooms_accom"))
            Catch ex As Exception
                _Err = _Err + "rooms_accom" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _requested_accom = Convert.ToString(_dr("requested_accom"))
            Catch ex As Exception
                _Err = _Err + "requested_accom" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _room_rate_accom = Convert.ToString(_dr("room_rate_accom"))
            Catch ex As Exception
                _Err = _Err + "room_rate_accom" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _fin_con_num = Convert.ToString(_dr("fin_con_num"))
            Catch ex As Exception
                _Err = _Err + "fin_con_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _fin_con_query = Convert.ToString(_dr("fin_con_query"))
            Catch ex As Exception
                _Err = _Err + "fin_con_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _fin_con_resp = Convert.ToString(_dr("fin_con_resp"))
            Catch ex As Exception
                _Err = _Err + "fin_con_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_not_num = Convert.ToString(_dr("pri_not_num"))
            Catch ex As Exception
                _Err = _Err + "pri_not_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_not_query = Convert.ToString(_dr("pri_not_query"))
            Catch ex As Exception
                _Err = _Err + "pri_not_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_not_resp = Convert.ToString(_dr("pri_not_resp"))
            Catch ex As Exception
                _Err = _Err + "pri_not_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _date_num = Convert.ToString(_dr("date_num"))
            Catch ex As Exception
                _Err = _Err + "date_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _date_query = Convert.ToString(_dr("date_query"))
            Catch ex As Exception
                _Err = _Err + "date_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _date_resp = Convert.ToString(_dr("date_resp"))
            Catch ex As Exception
                _Err = _Err + "date_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _phi_dis_num = Convert.ToString(_dr("phi_dis_num"))
            Catch ex As Exception
                _Err = _Err + "phi_dis_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _phi_dis_query = Convert.ToString(_dr("phi_dis_query"))
            Catch ex As Exception
                _Err = _Err + "phi_dis_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _phi_dis_resp = Convert.ToString(_dr("phi_dis_resp"))
            Catch ex As Exception
                _Err = _Err + "phi_dis_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _res_par_num = Convert.ToString(_dr("res_par_num"))
            Catch ex As Exception
                _Err = _Err + "res_par_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _res_par_query = Convert.ToString(_dr("res_par_query"))
            Catch ex As Exception
                _Err = _Err + "res_par_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _res_par_resp = Convert.ToString(_dr("res_par_resp"))
            Catch ex As Exception
                _Err = _Err + "res_par_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _date_sig_num = Convert.ToString(_dr("date_sig_num"))
            Catch ex As Exception
                _Err = _Err + "date_sig_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _date_sig_query = Convert.ToString(_dr("date_sig_query"))
            Catch ex As Exception
                _Err = _Err + "date_sig_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _date_sig_resp = Convert.ToString(_dr("date_sig_resp"))
            Catch ex As Exception
                _Err = _Err + "date_sig_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ack_num = Convert.ToString(_dr("ack_num"))
            Catch ex As Exception
                _Err = _Err + "ack_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ack_query = Convert.ToString(_dr("ack_query"))
            Catch ex As Exception
                _Err = _Err + "ack_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ack_resp = Convert.ToString(_dr("ack_resp"))
            Catch ex As Exception
                _Err = _Err + "ack_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admorl_num = Convert.ToString(_dr("admorl_num"))
            Catch ex As Exception
                _Err = _Err + "admorl_num" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admorl_query = Convert.ToString(_dr("admorl_query"))
            Catch ex As Exception
                _Err = _Err + "admorl_query" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admorl_resp = Convert.ToString(_dr("admorl_resp"))
            Catch ex As Exception
                _Err = _Err + "admorl_resp" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _prior_location = Convert.ToString(_dr("prior_location"))
            Catch ex As Exception
                _Err = _Err + "prior_location" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _condition_codes = Convert.ToString(_dr("condition_codes"))
            Catch ex As Exception
                _Err = _Err + "condition_codes" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _coa_scanned = Convert.ToString(_dr("coa_scanned"))
            Catch ex As Exception
                _Err = _Err + "coa_scanned" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _er_slip_scanned = Convert.ToString(_dr("er_slip_scanned"))
            Catch ex As Exception
                _Err = _Err + "er_slip_scanned" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _wc_scanned = Convert.ToString(_dr("wc_scanned"))
            Catch ex As Exception
                _Err = _Err + "wc_scanned" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _discharge_disposition = Convert.ToString(_dr("discharge_disposition"))
            Catch ex As Exception
                _Err = _Err + "discharge_disposition" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_status = Convert.ToString(_dr("pri_status"))
            Catch ex As Exception
                _Err = _Err + "pri_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_status = Convert.ToString(_dr("sec_status"))
            Catch ex As Exception
                _Err = _Err + "sec_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_status = Convert.ToString(_dr("ter_status"))
            Catch ex As Exception
                _Err = _Err + "ter_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_status = Convert.ToString(_dr("oth_status"))
            Catch ex As Exception
                _Err = _Err + "oth_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _validation_code = Convert.ToString(_dr("validation_code"))
            Catch ex As Exception
                _Err = _Err + "validation_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _address_dpv = Convert.ToString(_dr("address_dpv"))
            Catch ex As Exception
                _Err = _Err + "address_dpv" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _provider_source = Convert.ToString(_dr("provider_source"))
            Catch ex As Exception
                _Err = _Err + "provider_source" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _homeless = Convert.ToString(_dr("homeless"))
            Catch ex As Exception
                _Err = _Err + "homeless" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nonnetwkphys = Convert.ToString(_dr("nonnetwkphys"))
            Catch ex As Exception
                _Err = _Err + "nonnetwkphys" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _coa_signed = Convert.ToString(_dr("coa_signed"))
            Catch ex As Exception
                _Err = _Err + "coa_signed" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _admission_status = Convert.ToString(_dr("admission_status"))
            Catch ex As Exception
                _Err = _Err + "admission_status" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _transfer_fac_code = Convert.ToString(_dr("transfer_fac_code"))
            Catch ex As Exception
                _Err = _Err + "transfer_fac_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _discharge_letter = Convert.ToString(_dr("discharge_letter"))
            Catch ex As Exception
                _Err = _Err + "discharge_letter" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _npp_form = Convert.ToString(_dr("npp_form"))
            Catch ex As Exception
                _Err = _Err + "npp_form" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq1 = Convert.ToString(_dr("mspq1"))
            Catch ex As Exception
                _Err = _Err + "mspq1" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq2 = Convert.ToString(_dr("mspq2"))
            Catch ex As Exception
                _Err = _Err + "mspq2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq3 = Convert.ToString(_dr("mspq3"))
            Catch ex As Exception
                _Err = _Err + "mspq3" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq4 = Convert.ToString(_dr("mspq4"))
            Catch ex As Exception
                _Err = _Err + "mspq4" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq5 = Convert.ToString(_dr("mspq5"))
            Catch ex As Exception
                _Err = _Err + "mspq5" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq6 = Convert.ToString(_dr("mspq6"))
            Catch ex As Exception
                _Err = _Err + "mspq6" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq7 = Convert.ToString(_dr("mspq7"))
            Catch ex As Exception
                _Err = _Err + "mspq7" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq8 = Convert.ToString(_dr("mspq8"))
            Catch ex As Exception
                _Err = _Err + "mspq8" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq9 = Convert.ToString(_dr("mspq9"))
            Catch ex As Exception
                _Err = _Err + "mspq9" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq10 = Convert.ToString(_dr("mspq10"))
            Catch ex As Exception
                _Err = _Err + "mspq10" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq11 = Convert.ToString(_dr("mspq11"))
            Catch ex As Exception
                _Err = _Err + "mspq11" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq12 = Convert.ToString(_dr("mspq12"))
            Catch ex As Exception
                _Err = _Err + "mspq12" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq13 = Convert.ToString(_dr("mspq13"))
            Catch ex As Exception
                _Err = _Err + "mspq13" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq14 = Convert.ToString(_dr("mspq14"))
            Catch ex As Exception
                _Err = _Err + "mspq14" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq15 = Convert.ToString(_dr("mspq15"))
            Catch ex As Exception
                _Err = _Err + "mspq15" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq16 = Convert.ToString(_dr("mspq16"))
            Catch ex As Exception
                _Err = _Err + "mspq16" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq17 = Convert.ToString(_dr("mspq17"))
            Catch ex As Exception
                _Err = _Err + "mspq17" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq18 = Convert.ToString(_dr("mspq18"))
            Catch ex As Exception
                _Err = _Err + "mspq18" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq19 = Convert.ToString(_dr("mspq19"))
            Catch ex As Exception
                _Err = _Err + "mspq19" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq20 = Convert.ToString(_dr("mspq20"))
            Catch ex As Exception
                _Err = _Err + "mspq20" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mspq21 = Convert.ToString(_dr("mspq21"))
            Catch ex As Exception
                _Err = _Err + "mspq21" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _authorization_service_primary = Convert.ToString(_dr("authorization_service_primary"))
            Catch ex As Exception
                _Err = _Err + "authorization_service_primary" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _cpt1 = Convert.ToString(_dr("cpt1"))
            Catch ex As Exception
                _Err = _Err + "cpt1" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _cpt2 = Convert.ToString(_dr("cpt2"))
            Catch ex As Exception
                _Err = _Err + "cpt2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _cpt3 = Convert.ToString(_dr("cpt3"))
            Catch ex As Exception
                _Err = _Err + "cpt3" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _cpt4 = Convert.ToString(_dr("cpt4"))
            Catch ex As Exception
                _Err = _Err + "cpt4" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _medicare_hic = Convert.ToString(_dr("medicare_hic"))
            Catch ex As Exception
                _Err = _Err + "medicare_hic" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _bic_number = Convert.ToString(_dr("bic_number"))
            Catch ex As Exception
                _Err = _Err + "bic_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ccs_number = Convert.ToString(_dr("ccs_number"))
            Catch ex As Exception
                _Err = _Err + "ccs_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _tricare_number = Convert.ToString(_dr("tricare_number"))
            Catch ex As Exception
                _Err = _Err + "tricare_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _workerscomp_number = Convert.ToString(_dr("workerscomp_number"))
            Catch ex As Exception
                _Err = _Err + "workerscomp_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _certificate_number = Convert.ToString(_dr("certificate_number"))
            Catch ex As Exception
                _Err = _Err + "certificate_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _hmo_patientid = Convert.ToString(_dr("hmo_patientid"))
            Catch ex As Exception
                _Err = _Err + "hmo_patientid" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _medicalgroup1 = Convert.ToString(_dr("medicalgroup1"))
            Catch ex As Exception
                _Err = _Err + "medicalgroup1" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _healthplanname = Convert.ToString(_dr("healthplanname"))
            Catch ex As Exception
                _Err = _Err + "healthplanname" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _encounter_loc = Convert.ToString(_dr("encounter_loc"))
            Catch ex As Exception
                _Err = _Err + "encounter_loc" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _payor1_key = Convert.ToString(_dr("payor1_key"))
            Catch ex As Exception
                _Err = _Err + "payor1_key" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _payor2_key = Convert.ToString(_dr("payor2_key"))
            Catch ex As Exception
                _Err = _Err + "payor2_key" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _payor3_key = Convert.ToString(_dr("payor3_key"))
            Catch ex As Exception
                _Err = _Err + "payor3_key" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _casekey = Convert.ToString(_dr("casekey"))
            Catch ex As Exception
                _Err = _Err + "casekey" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _encounterkey = Convert.ToString(_dr("encounterkey"))
            Catch ex As Exception
                _Err = _Err + "encounterkey" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _advanced_directive_file = Convert.ToString(_dr("advanced_directive_file"))
            Catch ex As Exception
                _Err = _Err + "advanced_directive_file" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _rights_doc = Convert.ToString(_dr("rights_doc"))
            Catch ex As Exception
                _Err = _Err + "rights_doc" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_subscriber = Convert.ToString(_dr("patient_subscriber"))
            Catch ex As Exception
                _Err = _Err + "patient_subscriber" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _team = Convert.ToString(_dr("team"))
            Catch ex As Exception
                _Err = _Err + "team" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _broughtby = Convert.ToString(_dr("broughtby"))
            Catch ex As Exception
                _Err = _Err + "broughtby" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _aka_lname = Convert.ToString(_dr("aka_lname"))
            Catch ex As Exception
                _Err = _Err + "aka_lname" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _eligibilityflag1 = Convert.ToString(_dr("eligibilityflag1"))
            Catch ex As Exception
                _Err = _Err + "eligibilityflag1" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _eligibilityflag2 = Convert.ToString(_dr("eligibilityflag2"))
            Catch ex As Exception
                _Err = _Err + "eligibilityflag2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _eligibilityflag3 = Convert.ToString(_dr("eligibilityflag3"))
            Catch ex As Exception
                _Err = _Err + "eligibilityflag3" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _Medical_Benefits = Convert.ToString(_dr("Medical_Benefits"))
            Catch ex As Exception
                _Err = _Err + "Medical_Benefits" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _Medicare_Benefits = Convert.ToString(_dr("Medicare_Benefits"))
            Catch ex As Exception
                _Err = _Err + "Medicare_Benefits" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _visit_sequence = Convert.ToString(_dr("visit_sequence"))
            Catch ex As Exception
                _Err = _Err + "visit_sequence" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _authorization_service_secondary = Convert.ToString(_dr("authorization_service_secondary"))
            Catch ex As Exception
                _Err = _Err + "authorization_service_secondary" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _authorization_service_tertiary = Convert.ToString(_dr("authorization_service_tertiary"))
            Catch ex As Exception
                _Err = _Err + "authorization_service_tertiary" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nurse_unit = Convert.ToString(_dr("nurse_unit"))
            Catch ex As Exception
                _Err = _Err + "nurse_unit" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _newborn_indicator = Convert.ToString(_dr("newborn_indicator"))
            Catch ex As Exception
                _Err = _Err + "newborn_indicator" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _cob_date = Convert.ToString(_dr("cob_date"))
            Catch ex As Exception
                _Err = _Err + "cob_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_address = Convert.ToString(_dr("referring_doctor_address"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_city = Convert.ToString(_dr("referring_doctor_city"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_state = Convert.ToString(_dr("referring_doctor_state"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_zip = Convert.ToString(_dr("referring_doctor_zip"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_zip" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _referring_doctor_phone = Convert.ToString(_dr("referring_doctor_phone"))
            Catch ex As Exception
                _Err = _Err + "referring_doctor_phone" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _Primary_doctor_Id = Convert.ToString(_dr("Primary_doctor_Id"))
            Catch ex As Exception
                _Err = _Err + "Primary_doctor_Id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _Primary_doctor_LastName = Convert.ToString(_dr("Primary_doctor_LastName"))
            Catch ex As Exception
                _Err = _Err + "Primary_doctor_LastName" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _Primary_doctor_FirstName = Convert.ToString(_dr("Primary_doctor_FirstName"))
            Catch ex As Exception
                _Err = _Err + "Primary_doctor_FirstName" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _Primary_doctor_MiddleName = Convert.ToString(_dr("Primary_doctor_MiddleName"))
            Catch ex As Exception
                _Err = _Err + "Primary_doctor_MiddleName" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_doctor_address = Convert.ToString(_dr("primary_doctor_address"))
            Catch ex As Exception
                _Err = _Err + "primary_doctor_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_doctor_city = Convert.ToString(_dr("primary_doctor_city"))
            Catch ex As Exception
                _Err = _Err + "primary_doctor_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_doctor_state = Convert.ToString(_dr("primary_doctor_state"))
            Catch ex As Exception
                _Err = _Err + "primary_doctor_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_doctor_zip = Convert.ToString(_dr("primary_doctor_zip"))
            Catch ex As Exception
                _Err = _Err + "primary_doctor_zip" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _primary_doctor_phone = Convert.ToString(_dr("primary_doctor_phone"))
            Catch ex As Exception
                _Err = _Err + "primary_doctor_phone" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _outpatient_verify_flag = Convert.ToString(_dr("outpatient_verify_flag"))
            Catch ex As Exception
                _Err = _Err + "outpatient_verify_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _encounter_data_verify = Convert.ToString(_dr("encounter_data_verify"))
            Catch ex As Exception
                _Err = _Err + "encounter_data_verify" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _retirement_date = Convert.ToString(_dr("retirement_date"))
            Catch ex As Exception
                _Err = _Err + "retirement_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_location2 = Convert.ToString(_dr("patient_location2"))
            Catch ex As Exception
                _Err = _Err + "patient_location2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_location3 = Convert.ToString(_dr("patient_location3"))
            Catch ex As Exception
                _Err = _Err + "patient_location3" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_location4 = Convert.ToString(_dr("patient_location4"))
            Catch ex As Exception
                _Err = _Err + "patient_location4" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_location5 = Convert.ToString(_dr("patient_location5"))
            Catch ex As Exception
                _Err = _Err + "patient_location5" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_ins_benefit_plan = Convert.ToString(_dr("pri_ins_benefit_plan"))
            Catch ex As Exception
                _Err = _Err + "pri_ins_benefit_plan" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_ins_benefit_plan = Convert.ToString(_dr("sec_ins_benefit_plan"))
            Catch ex As Exception
                _Err = _Err + "sec_ins_benefit_plan" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_ins_benefit_plan = Convert.ToString(_dr("ter_ins_benefit_plan"))
            Catch ex As Exception
                _Err = _Err + "ter_ins_benefit_plan" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _oth_ins_benefit_plan = Convert.ToString(_dr("oth_ins_benefit_plan"))
            Catch ex As Exception
                _Err = _Err + "oth_ins_benefit_plan" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _quick_reg_flag = Convert.ToString(_dr("quick_reg_flag"))
            Catch ex As Exception
                _Err = _Err + "quick_reg_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _guarantor_number = Convert.ToString(_dr("guarantor_number"))
            Catch ex As Exception
                _Err = _Err + "guarantor_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_occupation = Convert.ToString(_dr("patient_occupation"))
            Catch ex As Exception
                _Err = _Err + "patient_occupation" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_middle_initial = Convert.ToString(_dr("pri_insured_middle_initial"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_middle_initial" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_middle_initial = Convert.ToString(_dr("sec_insured_middle_initial"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_middle_initial" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_middle_initial = Convert.ToString(_dr("ter_insured_middle_initial"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_middle_initial" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_insured_suffix = Convert.ToString(_dr("pri_insured_suffix"))
            Catch ex As Exception
                _Err = _Err + "pri_insured_suffix" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_insured_suffix = Convert.ToString(_dr("sec_insured_suffix"))
            Catch ex As Exception
                _Err = _Err + "sec_insured_suffix" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_insured_suffix = Convert.ToString(_dr("ter_insured_suffix"))
            Catch ex As Exception
                _Err = _Err + "ter_insured_suffix" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _patient_county_code = Convert.ToString(_dr("patient_county_code"))
            Catch ex As Exception
                _Err = _Err + "patient_county_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mn_required = Convert.ToString(_dr("mn_required"))
            Catch ex As Exception
                _Err = _Err + "mn_required" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _mn_completed = Convert.ToString(_dr("mn_completed"))
            Catch ex As Exception
                _Err = _Err + "mn_completed" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_street_address = Convert.ToString(_dr("nearest_relative_street_address"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_street_address" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_address2 = Convert.ToString(_dr("nearest_relative_address2"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_address2" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_city = Convert.ToString(_dr("nearest_relative_city"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_city" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_state = Convert.ToString(_dr("nearest_relative_state"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_state" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_zip_code = Convert.ToString(_dr("nearest_relative_zip_code"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_zip_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _nearest_relative_country_code = Convert.ToString(_dr("nearest_relative_country_code"))
            Catch ex As Exception
                _Err = _Err + "nearest_relative_country_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_subscriber_name_last = Convert.ToString(_dr("pri_payor_subscriber_name_last"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_subscriber_name_last" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_subscriber_name_first = Convert.ToString(_dr("pri_payor_subscriber_name_first"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_subscriber_name_first" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_subscriber_name_middle = Convert.ToString(_dr("pri_payor_subscriber_name_middle"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_subscriber_name_middle" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_subscriber_id = Convert.ToString(_dr("pri_payor_subscriber_id"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_subscriber_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_susbcriber_dob = Convert.ToString(_dr("pri_payor_susbcriber_dob"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_susbcriber_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_subscriber_gender = Convert.ToString(_dr("pri_payor_subscriber_gender"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_subscriber_gender" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_plan_type = Convert.ToString(_dr("pri_payor_plan_type"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_subscriber_name_last = Convert.ToString(_dr("sec_payor_subscriber_name_last"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_subscriber_name_last" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_subscriber_name_first = Convert.ToString(_dr("sec_payor_subscriber_name_first"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_subscriber_name_first" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_subscriber_name_middle = Convert.ToString(_dr("sec_payor_subscriber_name_middle"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_subscriber_name_middle" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_subscriber_id = Convert.ToString(_dr("sec_payor_subscriber_id"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_subscriber_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_susbcriber_dob = Convert.ToString(_dr("sec_payor_susbcriber_dob"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_susbcriber_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_subscriber_gender = Convert.ToString(_dr("sec_payor_subscriber_gender"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_subscriber_gender" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_plan_type = Convert.ToString(_dr("sec_payor_plan_type"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_subscriber_name_last = Convert.ToString(_dr("ter_payor_subscriber_name_last"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_subscriber_name_last" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_subscriber_name_first = Convert.ToString(_dr("ter_payor_subscriber_name_first"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_subscriber_name_first" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_subscriber_name_middle = Convert.ToString(_dr("ter_payor_subscriber_name_middle"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_subscriber_name_middle" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_subscriber_id = Convert.ToString(_dr("ter_payor_subscriber_id"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_subscriber_id" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_susbcriber_dob = Convert.ToString(_dr("ter_payor_susbcriber_dob"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_susbcriber_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_subscriber_gender = Convert.ToString(_dr("ter_payor_subscriber_gender"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_subscriber_gender" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_plan_type = Convert.ToString(_dr("ter_payor_plan_type"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_plan_type" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_code = Convert.ToString(_dr("pri_payor_code"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_code = Convert.ToString(_dr("sec_payor_code"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_code = Convert.ToString(_dr("ter_payor_code"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_code" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_mcra_inactive = Convert.ToString(_dr("pri_mcra_inactive"))
            Catch ex As Exception
                _Err = _Err + "pri_mcra_inactive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_mcra_inactive = Convert.ToString(_dr("sec_mcra_inactive"))
            Catch ex As Exception
                _Err = _Err + "sec_mcra_inactive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_mcra_inactive = Convert.ToString(_dr("ter_mcra_inactive"))
            Catch ex As Exception
                _Err = _Err + "ter_mcra_inactive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_mcrb_inactive = Convert.ToString(_dr("pri_mcrb_inactive"))
            Catch ex As Exception
                _Err = _Err + "pri_mcrb_inactive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_mcrb_inactive = Convert.ToString(_dr("sec_mcrb_inactive"))
            Catch ex As Exception
                _Err = _Err + "sec_mcrb_inactive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_mcrb_inactive = Convert.ToString(_dr("ter_mcrb_inactive"))
            Catch ex As Exception
                _Err = _Err + "ter_mcrb_inactive" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_plan_number = Convert.ToString(_dr("pri_payor_plan_number"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_plan_number = Convert.ToString(_dr("sec_payor_plan_number"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_plan_number = Convert.ToString(_dr("ter_payor_plan_number"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_dep_name_last = Convert.ToString(_dr("pri_payor_dep_name_last"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_dep_name_last" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_dep_name_first = Convert.ToString(_dr("pri_payor_dep_name_first"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_dep_name_first" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_dep_name_middle = Convert.ToString(_dr("pri_payor_dep_name_middle"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_dep_name_middle" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_dep_dob = Convert.ToString(_dr("pri_payor_dep_dob"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_dep_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_dep_gender = Convert.ToString(_dr("pri_payor_dep_gender"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_dep_gender" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_dep_name_last = Convert.ToString(_dr("sec_payor_dep_name_last"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_dep_name_last" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_dep_name_first = Convert.ToString(_dr("sec_payor_dep_name_first"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_dep_name_first" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_dep_name_middle = Convert.ToString(_dr("sec_payor_dep_name_middle"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_dep_name_middle" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_dep_dob = Convert.ToString(_dr("sec_payor_dep_dob"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_dep_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_dep_gender = Convert.ToString(_dr("sec_payor_dep_gender"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_dep_gender" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_dep_name_last = Convert.ToString(_dr("ter_payor_dep_name_last"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_dep_name_last" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_dep_name_first = Convert.ToString(_dr("ter_payor_dep_name_first"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_dep_name_first" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_dep_name_middle = Convert.ToString(_dr("ter_payor_dep_name_middle"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_dep_name_middle" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_dep_dob = Convert.ToString(_dr("ter_payor_dep_dob"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_dep_dob" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_dep_gender = Convert.ToString(_dr("ter_payor_dep_gender"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_dep_gender" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_res_plan_number = Convert.ToString(_dr("pri_payor_res_plan_number"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_res_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_res_plan_name = Convert.ToString(_dr("pri_payor_res_plan_name"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_res_plan_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_res_group_number = Convert.ToString(_dr("pri_payor_res_group_number"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_res_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_res_group_name = Convert.ToString(_dr("pri_payor_res_group_name"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_res_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_hmo_plan_name = Convert.ToString(_dr("pri_payor_hmo_plan_name"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_hmo_plan_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_res_plan_number = Convert.ToString(_dr("sec_payor_res_plan_number"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_res_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_res_plan_name = Convert.ToString(_dr("sec_payor_res_plan_name"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_res_plan_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_res_group_number = Convert.ToString(_dr("sec_payor_res_group_number"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_res_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_res_group_name = Convert.ToString(_dr("sec_payor_res_group_name"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_res_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_hmo_plan_name = Convert.ToString(_dr("sec_payor_hmo_plan_name"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_hmo_plan_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_res_plan_number = Convert.ToString(_dr("ter_payor_res_plan_number"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_res_plan_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_res_plan_name = Convert.ToString(_dr("ter_payor_res_plan_name"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_res_plan_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_res_group_number = Convert.ToString(_dr("ter_payor_res_group_number"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_res_group_number" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_res_group_name = Convert.ToString(_dr("ter_payor_res_group_name"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_res_group_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_hmo_plan_name = Convert.ToString(_dr("ter_payor_hmo_plan_name"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_hmo_plan_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_mcr_hmo = Convert.ToString(_dr("pri_payor_mcr_hmo"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_mcr_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_mcr_hmo = Convert.ToString(_dr("sec_payor_mcr_hmo"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_mcr_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_mcr_hmo = Convert.ToString(_dr("ter_payor_mcr_hmo"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_mcr_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_mcd_hmo = Convert.ToString(_dr("pri_payor_mcd_hmo"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_mcd_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_mcd_hmo = Convert.ToString(_dr("sec_payor_mcd_hmo"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_mcd_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_mcd_hmo = Convert.ToString(_dr("ter_payor_mcd_hmo"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_mcd_hmo" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_msp_flag = Convert.ToString(_dr("pri_payor_msp_flag"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_msp_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_msp_flag = Convert.ToString(_dr("sec_payor_msp_flag"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_msp_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_msp_flag = Convert.ToString(_dr("ter_payor_msp_flag"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_msp_flag" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_plan_sponser = Convert.ToString(_dr("pri_plan_sponser"))
            Catch ex As Exception
                _Err = _Err + "pri_plan_sponser" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_plan_sponser = Convert.ToString(_dr("sec_plan_sponser"))
            Catch ex As Exception
                _Err = _Err + "sec_plan_sponser" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_plan_sponser = Convert.ToString(_dr("ter_plan_sponser"))
            Catch ex As Exception
                _Err = _Err + "ter_plan_sponser" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_other_payor = Convert.ToString(_dr("pri_other_payor"))
            Catch ex As Exception
                _Err = _Err + "pri_other_payor" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_other_payor = Convert.ToString(_dr("sec_other_payor"))
            Catch ex As Exception
                _Err = _Err + "sec_other_payor" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_other_payor = Convert.ToString(_dr("ter_other_payor"))
            Catch ex As Exception
                _Err = _Err + "ter_other_payor" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_mcr_hospice = Convert.ToString(_dr("pri_payor_mcr_hospice"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_mcr_hospice" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_mcr_hospice_date = Convert.ToString(_dr("pri_payor_mcr_hospice_date"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_mcr_hospice_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_mcr_hospice = Convert.ToString(_dr("sec_payor_mcr_hospice"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_mcr_hospice" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_mcr_hospice_date = Convert.ToString(_dr("sec_payor_mcr_hospice_date"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_mcr_hospice_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_mcr_hospice = Convert.ToString(_dr("ter_payor_mcr_hospice"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_mcr_hospice" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_mcr_hospice_date = Convert.ToString(_dr("ter_payor_mcr_hospice_date"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_mcr_hospice_date" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _pri_payor_pcp_name = Convert.ToString(_dr("pri_payor_pcp_name"))
            Catch ex As Exception
                _Err = _Err + "pri_payor_pcp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _sec_payor_pcp_name = Convert.ToString(_dr("sec_payor_pcp_name"))
            Catch ex As Exception
                _Err = _Err + "sec_payor_pcp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try


            Try
                _ter_payor_pcp_name = Convert.ToString(_dr("ter_payor_pcp_name"))
            Catch ex As Exception
                _Err = _Err + "ter_payor_pcp_name" + "|" + ex.Message + vbCrLf
                r = -1
            End Try






            If r = 0 Then


                _isBuilt = True


            End If


            Return r


        End Function


        Public Sub BuildMessages()

            _RuleMsg.Add(1001, "Patients last name may only contain letters and must be in upper case. There cannot be a blank first position in the Patient LASTNAME field.")

            _RuleMsg.Add(1002, "Patients first name may only contain letters and must be in upper case. First Name cannot begin with a space.")

            _RuleMsg.Add(1003, "PATIENTS MIDDLE NAME CONTAINS INVALID CHARACTERS")

            _RuleMsg.Add(1004, "Patients Marital Status is Required may not be U or UNK")

            _RuleMsg.Add(1005, "RACE OF U or UN IS INCORRECT")

            _RuleMsg.Add(1006, "RELIGION UN IS INCORRECT")

            _RuleMsg.Add(1007, "PATIENTS GENDER CANNOT BE U or UN")

            _RuleMsg.Add(1008, "Patients address is required and must consist of alpha and numeric characters. Space in the address and Dash in the zip code are the only exception. Zip 99999 is not acceptable.")

            _RuleMsg.Add(1009, "PATIENT PHONE MUST BE  ALL NUMERICS AND MAY NOT BE THE SAME SINGLE DIGIT REPEATED EXCEPT 0s")

            _RuleMsg.Add(1010, "PATIENT SSN MUST BE NINE NUMBERS UNLESS IT IS BLANK. PATIENT SSN CANNOT BE ALL THE SAME VALUE CAN NOT INCLUDE ANY ALPHA CHARACTERS")

            _RuleMsg.Add(1011, "PATIENTS BIRTHDATE IS INVALID MUST BE 8 DIGITS")

            _RuleMsg.Add(1012, "PATIENTS EMPLOYER PHONE NUMBER MUST BE TEN OR THREE DIGITS WHEN VALUED. PATIENTS EMPLOYER PHONE NUMBER MUST BE ALL NUMERICS")

            _RuleMsg.Add(1013, "PATIENTS EMPLOYER NAME IS REQUIRED")

            _RuleMsg.Add(1015, "Patient employer can not have punctuations or special characters.")

            _RuleMsg.Add(1016, "PATIENTS MIDDLE NAME CONTAINS INVALID CHARACTERS.")

            _RuleMsg.Add(1017, "PATIENTS DOB CANNOT BE IN THE FUTURE.")

            _RuleMsg.Add(1018, "PATIENT EMPLOYER INFORMATION IS REQUIRED")

            _RuleMsg.Add(1019, "PATIENT OTHER PHONE MUST BE  ALL NUMERICS AND MAY NOT BE THE SAME SINGLE DIGIT REPEATED EXCEPT 0s")

            _RuleMsg.Add(1020, "If state equal AB BC MB NB NL NS NT NU ON PE PQ QC SK YT or YU zipcode should be A#A#A# and patients complete address street city and state are required")

            _RuleMsg.Add(1021, "If state equals VI zipcode should start with  008 and can only be a 5 digit zipcode. Patients complete address street city and state are required")

            _RuleMsg.Add(2001, "THE GUARANTORS FIRST NAME CONTAINS A VALUE OTHER THAN AZ.GUARANTORS COMPLETE NAME SHOULD BE IN ALL CAPS")

            _RuleMsg.Add(2002, "THE GUARANTORS LAST NAME CONTAINS INVALID CHARACTERS. NOTE:CHECK FOR A  BLANK IN THE FIRST POSITION AND FOR ANY DIGITS AND SPECIAL CHARACTERS OTHER THAN  A DASH.GUARANTORS COMPLETE NAME SHOULD BE IN ALL CAPS")

            _RuleMsg.Add(2005, "Guarantor  Address contains invalid characters.  Alpha or Numeric are the only acceptable characters")

            _RuleMsg.Add(2006, "THE GUARANTORS PHONE # CANNOT HAVE THE SAME DIGIT REPEATED WITH THE EXCEPTION OF 0000000000")

            _RuleMsg.Add(2007, "IF THE GUARANTORS RELATIONSHIP = Self then PATIENT LAST NAME FIRST NAME must equal GUAR LAST NAME FIRST NAME")

            _RuleMsg.Add(2008, "If guar = Self and patient age < 18 then error")

            _RuleMsg.Add(2009, "GUARANTOR INFORMATION NAME STREET ADDRESS CITY STATE ZIP PHONE MUST BE PRESENT FOR PATIENT AGE <18")

            _RuleMsg.Add(2010, "If Guarantor relationship = Self the guarantor address city state and zip must match patients address city state and zip.")

            _RuleMsg.Add(2011, "If Financial class = WC then Guarantor can not = patient Self")

            _RuleMsg.Add(2012, "If guarantor relationship = Employee then guarantor name must be XNAME OF EMPLOYER")

            _RuleMsg.Add(2014, "A 5 digit zipcode cannot be entered as 99999 for the Guarnantor address.")

            _RuleMsg.Add(2015, "If Guarantor Relationship to Patient is not the patient then Guarantors name address phone SS and employer info must be filled in")

            _RuleMsg.Add(2016, "The Guarantor Social Security number is incorrect.  If the Guarantor is the patient based on the relationship code then the Social Security number should be the same as the patients Social Security.")

            _RuleMsg.Add(2017, "The Guarantor phone number is incorrect.  If the Guarantor is the patient based on the relationship code then phone number should be the same as the patient phone number.")

            _RuleMsg.Add(2018, "Guarantor Zip Code is blank.  Please populate the field with a 9 digit zip code.")

            _RuleMsg.Add(2019, "Guarantors employer information is required")

            _RuleMsg.Add(2020, "Guarantors employer does not match the patients employer when relationship to patient is self")

            _RuleMsg.Add(2021, "Guarantors phone number is required")

            _RuleMsg.Add(2022, "Guarantors address is required")

            _RuleMsg.Add(3001, "Emergency Contacts last name may only contain uppercase letters.  No spaces or special characters are allowed. Please check for a space at the beginning of the name.")

            _RuleMsg.Add(3002, "Emergency Contacts first name may only contain uppercase letters.  Please check for a space at the beginning of the name.")

            _RuleMsg.Add(3005, "EMERGENCY CONTACT PHONE NUMBER MUST BE TEN DIGITS AND NUMERICS ONLY")

            _RuleMsg.Add(3015, "Next of Kins last name may only contain uppercase letters.  No spaces or special characters are allowed. Please check for a space at the beginning of the name.")

            _RuleMsg.Add(3018, "Emergency Contact is required")

            _RuleMsg.Add(3019, "Emergency Contacts phone number is required")

            _RuleMsg.Add(3021, "Next of Kins phone number is required")

            _RuleMsg.Add(3022, "Phone number must not contain any other characters besides  numbers and must be nine digits")

            _RuleMsg.Add(3023, "The only exceptable format is NONE last name Given by patient first name")

            _RuleMsg.Add(3024, "The only exceptable format is NONE last name Given by patient first name")

            _RuleMsg.Add(4002, "Account must have attending physician Physician may not be UKN")

            _RuleMsg.Add(4003, "Acct is more than 32 days old acct needs closed and a new one created.")

            _RuleMsg.Add(5001, "THE PRIMARY SUBSCRIBERS LAST NAME CONTAINS INVALID CHARACTERS. NOTE: CHECK FOR A BLANK IN THE  FIRST POSITION AND FOR ANY DIGITS AND SPECIAL CHARACTERS  OTHER THAN A DASH")

            _RuleMsg.Add(5003, "THE PRIMARY INSURANCE SUBSCRIBERS DOB IS INVALID OR BLANK MUST BE 8 DIGITS ALL NUMERICS")

            _RuleMsg.Add(5004, "THE PATIENTS  RELATIONSHIP CODE TO THE PRIMARY SUBSCRIBER IS REQUIRED")

            _RuleMsg.Add(5006, "PRIMARY INSURANCE PLAN PHONE NUMBER MUST BE ALL NUMERICS AND 10 DIGITS UNLESS THE PRIMARY PLAN CODE IS Self Pay")

            _RuleMsg.Add(5007, "PRIMARY POLICY NUMBER CANNOT BE  THE SAME SINGLE DIGIT REPEATED")

            _RuleMsg.Add(5008, "PRIMARY POLICY NUMBER CANNOT BE  THE SAME AS THE PLAN CODE")

            _RuleMsg.Add(5009, "PRIMARY POLICY NUMBER CANNOT BE  THE SAME AS THE GROUP NUMBER.")

            _RuleMsg.Add(5010, "THE PRIMARY INSURANCE GROUP NUMBER CANNOT BE THE SAME NUMBER REPEATED.")

            _RuleMsg.Add(5011, "IF FINANCIAL CLASS = MCD THEN PRIMARY POLICY NUMBER MUST BE 8 DIGIT NUMERIC")

            _RuleMsg.Add(5012, "INSURANCE PLAN CODE Self Pay CANNOT BE PRIMARY WITH ACTIVE SECONDARY COVERAGE")

            _RuleMsg.Add(5014, "PRIMARY INSURANCE POLICY NUMBER IS REQUIRED UNLESS INSURANCE IS SP OR WC")

            _RuleMsg.Add(5015, "CAN NOT HAVE ANY SECONDARY INSURANCE WHEN FINANCIAL CLASS IS SP OR MCD")

            _RuleMsg.Add(5016, "If patient DOB does not equal subscriber DOB than relationship cannot be Self")

            _RuleMsg.Add(5017, "CHN is not a valid carrier. Please select the correct carrier.")

            _RuleMsg.Add(5018, "Subscriber Relationship for Medicare and Medicaid must always  be self")

            _RuleMsg.Add(5019, "MVA /WC/ Misc WC carriers should be primary")

            _RuleMsg.Add(5020, "When Medicaid or Managed Medicaid financial classes are selected and there is more than one carrier these classes must always represent the last payer.")

            _RuleMsg.Add(5021, "PRIMARY POLICY NUMBER CONTAINS A SPACE OR INVALID CHARACTERS")

            _RuleMsg.Add(5022, "Primary Medicare plans must have an alpha at 1st or 10th position. The plan must be 1 to 3 alpha followed by 9 digits or 9 digits followed by 1 alpha and an optional 1 digit or alpha. ie; A123456789 or ABC123456789 or 123456789A or 123456789C1 or 12")

            _RuleMsg.Add(5023, "Primary insurance plans policy number can not be NONE")

            _RuleMsg.Add(5024, "Primary Group Number is required.")

            _RuleMsg.Add(5025, "If Primary Insurance plan is 31415 10974 10192 F 7930 and 1052 subscriber must equal the patient.")

            _RuleMsg.Add(5026, "Primary Subscriber relation must be SELF when Primary Plan is M51.")

            _RuleMsg.Add(5027, "If policy number begins with HH then plan code should be loaded as Health Plans Inc 665")

            _RuleMsg.Add(5028, "If policy number begins with HP then plan code should be loaded as Health Plans Inc 665")

            _RuleMsg.Add(5029, "If plan code is 3 7930 35 10160 or 1415 Authorization number field must be 6 digits and can not contain Alpha or Special characters for Maine Care.")

            _RuleMsg.Add(5030, "Mercy is NOT innetwork for this plan  XVH and group number starts with 00B885")

            _RuleMsg.Add(5031, "Mercy is NOT innetwork for this plan  policy number starting with VBI VBG VBC VBE VBD VBF XVN or XVO.")

            _RuleMsg.Add(6012, "SELF PAY CANNOT BE SECONDARY.")

            _RuleMsg.Add(6022, "Secondary Medicare plans must have an alpha at 1st or 10th position. The plan must be 1 to 3 alpha followed by 9 digits or 9 digits followed by 1 alpha and an optional 1 digit or alpha. ie; A123456789 or ABC123456789 or 123456789A or 123456789C1 or")

            _RuleMsg.Add(6024, "Secondary Group Number is required.")

            _RuleMsg.Add(6025, "If Secondary Insurance plan is 31415 10974 10192 F 7930 and 1052 subscriber must equal the patient.")

            _RuleMsg.Add(6027, "If policy number begins with HH then plan code should be loaded as Health Plans Inc 665")

            _RuleMsg.Add(6028, "If policy number begins with HH then plan code should be loaded as Health Plans Inc 665")

            _RuleMsg.Add(6029, "If plan code is 3 7930 35 10160 or 1415 Authorization number field must be 6 digits and can not contain Alpha or Special characters for Maine Care.")

            _RuleMsg.Add(6030, "Mercy is NOT innetwork for this plan  XVH and group number starts with 00B885")

            _RuleMsg.Add(7012, "SELF PAY CANNOT BE TERTIARY.")

            _RuleMsg.Add(7022, "Tertiary Medicare plans must have an alpha at 1st or 10th position. The plan must be 1 to 3 alpha followed by 9 digits or 9 digits followed by 1 alpha and an optional 1 digit or alpha. ie; A123456789 or ABC123456789 or 123456789A or 123456789C1 or 1")

            _RuleMsg.Add(7024, "Tertiary Group Number is required.")

            _RuleMsg.Add(7025, "If Tertiary Insurance plan is 31415 10974 10192 F 7930 and 1052 subscriber must equal the patient.")

            _RuleMsg.Add(7027, "If policy number begins with HH then plan code should be loaded as Health Plans Inc 665")

            _RuleMsg.Add(7028, "If policy number begins with HH then plan code should be loaded as Health Plans Inc 665")

            _RuleMsg.Add(7029, "If plan code is 3 7930 35 10160 or 1415 Authorization number field must be 6 digits and can not contain Alpha or Special characters for Maine Care.")

            _RuleMsg.Add(7030, "Mercy is NOT innetwork for this plan  XVH and group number starts with 00B885")

            _RuleMsg.Add(8012, "SELF PAY CANNOT BE OTHER.")

            _RuleMsg.Add(8502, "Primary Eligibility Results Failed")

            _RuleMsg.Add(8504, "Secondary Eligibility Results Failed")

            _RuleMsg.Add(8505, "Tertiary Eligibility Results Failed")

            _RuleMsg.Add(8506, "Primary Eligibility Results returns Review")

            _RuleMsg.Add(8507, "Secondary Eligibility Results Returned REVIEW.")

            _RuleMsg.Add(8508, "Tertiary Eligibility Results Returned REVIEW.")

            _RuleMsg.Add(8509, "Please correct the Primary Subscriber ID based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8510, "Please correct the Secondary Insurance Subscriber ID based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8511, "Please correct the Tertiary Insurance Subscriber ID based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8512, "Please correct the primary subscriber DOB based off the eligibility response.  The two should match.  Advise patient to contact payer to update DOB.")

            _RuleMsg.Add(8513, "Please correct the Secondary Insurance Subscriber DOB based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8514, "Please correct the Tertiary Insurance Subscriber DOB based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8515, "Please correct the Primary Subscriber Gender based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8516, "Please correct the Secondary Insurance Subscriber Gender based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8517, "Please correct the Tertiary Insurance Subscriber Gender based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8518, "Please correct the Primary Insureds first name or name based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8519, "Please correct the Secondary Insureds first name or name based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8520, "Please correct the Tertiary Insureds first name or name based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8521, "Please correct the Primary Insureds last name or name based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8522, "Please correct the Secondary Insureds last name or name based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8523, "Please correct the Tertiary Insureds last name or name based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8524, "If Patient type is I and Medicare response states inactive Part A coverage Insurance plan must equal 37")

            _RuleMsg.Add(8525, "If Patient type is I and Medicare response states inactive Part A coverage Insurance plan must equal 37")

            _RuleMsg.Add(8526, "If Patient type is I and Medicare response states inactive Part A coverage Insurance plan must equal 37")

            _RuleMsg.Add(8527, "If Patient type is B E F G J K O R or S and Medicare response states inactive Part B coverage Insurance plan MUST NOT equal 37 69 10015")

            _RuleMsg.Add(8528, "If Patient type is B E F G J K O R or S and Medicare response states inactive Part B coverage Insurance plan must not equal 37 69 10015")

            _RuleMsg.Add(8529, "If Patient type is B E F G J K O R or S and Medicare response states inactive Part B coverage Insurance plan must not equal 37 69 10015")

            _RuleMsg.Add(8530, "If Medicare response states Part AandB inactive Insurance Plan CAN NOT equal 1429 10014 10975 37 69 38 10015 or 10978.")

            _RuleMsg.Add(8534, "If Medicare response states Patient is currently  in a hospice benefit period.")

            _RuleMsg.Add(8535, "If Medicare response states Patient is currently  in a hospice benefit period.")

            _RuleMsg.Add(8536, "If Medicare response states Patient is currently  in a hospice benefit period.")

            _RuleMsg.Add(8537, "Primary Insurance Eligibility is Inactive")

            _RuleMsg.Add(8538, "Secondary Insurance Eligibility is Inactive")

            _RuleMsg.Add(8539, "Tertiary Insurance Eligibility is Inactive")

            _RuleMsg.Add(8540, "Primary Eligibility returned N42.")

            _RuleMsg.Add(8541, "Secondary Eligibility returned N42.")

            _RuleMsg.Add(8542, "Tertiary Eligibility returned N42.")

            _RuleMsg.Add(8543, "Primary Insurance Eligibility returns Medicare as Secondary Payor. Please correct the insurance on the account.")

            _RuleMsg.Add(8544, "If Medicare response states Patient is enrolled in a Medicare Advantage plan Insurance plan can not be 1429 10014 10975 37 69 38 10015 or 10978.")

            _RuleMsg.Add(8549, "If Insurance plan 3 35 1052 1415 10974 10192 OR 10160 and Eligibility returns Maincare with PCCMAdult and Children Services Patient is enrolled in a  Managedcare. Insurance plan should be 7930.")

            _RuleMsg.Add(8550, "If Secondary Insurance plan 3 35 1052 1415 10974 10192 OR 10160 and Eligibility returns Maincare with PCCMAdult and Children Services Patient is enrolled in a  Managedcare. Inaurance plan should be 7930.")

            _RuleMsg.Add(8551, "If Tertiary Insurance plan 3 35 1052 1415 10974 10192 OR 10160 and Eligibility returns Maincare with PCCMAdult and Children Services Patient is enrolled in a  Managedcare. Inaurance plan should be 7930.")

            _RuleMsg.Add(8552, "If Primary Insurance plan 3 35 1052 1415 7930  10974 10192 10160 and Eligibility returns Maincare with Maine RX Patient has RX coverage only. Insurance plan should be 45 Self pay.")

            _RuleMsg.Add(8555, "Please correct the Primary Insureds Middle Name or Initial based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8556, "Please correct the Secondary Insureds Middle Name or Initial based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8557, "Please correct the Tertiary Insureds Middle Name or Initial based off of the eligibility response.  The two should match.")

            _RuleMsg.Add(8558, "Please correct the Primary Care Physician name based off of the Primary Insurance eligibility response.  The two should match.")

            _RuleMsg.Add(9501, "Guarantor Address is E")

            _RuleMsg.Add(9502, "Guarantor address is C0")

            _RuleMsg.Add(9503, "GUARANTOR STREET NUMBER ENTERED HAS MORE THAN ONE UNIT OR BUILDING ASSOCIATED WITH IT")

            _RuleMsg.Add(9505, "Guarantor address is C4")

            _RuleMsg.Add(9506, "Guarantor address is M")

            _isMessagesBuilt = True
        End Sub
        Public Function RunRules() As Integer
            Dim R As Integer = 0

            If _isMessagesBuilt = False Then

                BuildMessages()

            End If
            If _isBuilt = False Then
                ParseRuleDataRow()
            End If
            Try
                '

                Dim r_1001 As Integer = 0
                r_1001 = vb_1001_Pat_LName(_patient_last_name)

                _RuleResults.Add(1001, r_1001)
            Catch ex As Exception
                _Err = _Err + "1001_Pat_LName" + "|" + ex.Message + vbCrLf

                R = -1

                _RuleResults.Add(1001, -50000)
            End Try






            Return r
        End Function
        'Begin RuleId =1001  RuleName=1001_Pat_LName
        'Patient's last name may only contain letters and must be in upper case. There cannot be a blank first position in the Patient LASTNAME field.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Function vb_1001_Pat_LName(ByVal patient_last_name As String) As Integer

            Dim E As Integer = 1
            'rule_id = 1001
            'rule_name = 1001_Pat_LName
            'rule_description = Patient's last name may only contain letters and must be in upper case. There cannot be a blank first position in the Patient LASTNAME field.




            'BEGIN decs***************************************************
            '*
            Dim _LName As String
            Dim _patient_last_name As String = patient_last_name
            '*
            'END decs*****************************************************


            'BEGIN Sets***************************************************
            '*
            _LName = patient_last_name
            '*
            'END Sets*****************************************************




            'BEGIN Sets***************************************************
            '*'
            '*
            'END Sets*****************************************************


            'BEGIN assignments***************************************************
            '*
            Dim objReg As Regex = New Regex("^[^A-Z]", RegexOptions.IgnoreCase)


            '*
            'END assignments*****************************************************


            'BEGIN Code Body***************************************************
            Try
                Dim objRegMatch As Match = objReg.Match(_LName)

                If objregmatch.success Then

                    e = -1001

                Else

                    e = 1

                End If

                Return E

            Catch ex As Exception
            End Try
            '*
            'END Code Body*****************************************************


            'BEGIN Destory Objects***************************************************
            '*
            '*
            'END Destory Objects******************************************************


            Return E


        End Function

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'End RuleId =1001  RuleName=1001_Pat_LName






    End Class
End Namespace
