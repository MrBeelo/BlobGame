#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
223,
235,
236,
237,
238,
239,
240,
241,
242,
243,
246,
247,
248,
419,
420,
421,
450,
451,
452,
472,
473,
474,
475,
592,
593,
594,
595,
598,
634,
635,
636,
639,
641,
643,
645,
650,
658,
659,
660,
661,
662,
663,
664,
665,
666,
667,
668,
669,
670,
671,
672,
673,
674,
676,
677,
678,
679,
680,
681,
682,
774,
775,
776,
777,
778,
779,
780,
781,
782,
783,
784,
785,
786,
787,
788,
789,
790,
792,
793,
794,
795,
796,
797,
798,
865,
866,
933,
940,
943,
945,
950,
951,
953,
954,
958,
959,
961,
963,
964,
967,
968,
969,
972,
974,
977,
979,
981,
990,
1057,
1059,
1061,
1071,
1072,
1073,
1074,
1076,
1083,
1084,
1085,
1086,
1087,
1095,
1096,
1097,
1101,
1102,
1104,
1108,
1109,
1110,
1394,
1585,
1586,
10337,
10338,
10340,
10341,
10342,
10343,
10344,
10346,
10348,
10350,
10351,
10362,
10364,
10369,
10371,
10373,
10375,
10426,
10427,
10429,
10430,
10431,
10432,
10433,
10435,
10437,
11529,
11533,
11535,
11536,
11537,
11538,
11856,
11857,
11858,
11859,
11879,
11880,
11881,
11883,
11931,
12000,
12002,
12004,
12013,
12014,
12015,
12016,
12017,
12466,
12467,
12472,
12473,
12506,
12526,
12533,
12540,
12551,
12554,
12579,
12661,
12672,
12674,
12675,
12676,
12683,
12697,
12717,
12718,
12726,
12728,
12735,
12736,
12739,
12741,
12746,
12754,
12755,
12762,
12764,
12775,
12778,
12781,
12782,
12783,
12794,
12803,
12809,
12810,
12811,
12813,
12814,
12832,
12834,
12849,
12870,
12871,
12896,
12901,
12931,
12932,
13446,
13447,
13474,
13488,
13582,
13583,
13797,
13798,
13805,
13806,
13807,
13813,
13910,
14391,
14392,
14749,
14754,
14764,
16259,
16280,
16282,
16284,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal (int);
int ves_icall_System_Array_IsValueOfElementTypeInternal (int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy (int,int,int,int,int);
int ves_icall_System_Array_GetLengthInternal_raw (int,int,int);
int ves_icall_System_Array_GetLowerBoundInternal_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
void ves_icall_System_Array_GetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_InitializeInternal_raw (int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
void ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
void ves_icall_System_Enum_InternalBoxEnum_raw (int,int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_Exit (int);
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
void ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw (int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxToNullable_raw (int,int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_System_RuntimeType_AllocateValueType_raw (int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw (int,int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_LoaderAllocatorScout_Destroy (int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodBodyInternal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_AssemblyExtensions_ApplyUpdateEnabled (int);
int ves_icall_AssemblyExtensions_GetApplyUpdateCapabilities_raw (int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_System_Diagnostics_StackFrame_GetFrameInfo (int,int,int,int,int,int,int,int);
void ves_icall_System_Diagnostics_StackTrace_GetTrace (int,int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 223,
ves_icall_System_Array_InternalCreate,
// token 235,
ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal,
// token 236,
ves_icall_System_Array_IsValueOfElementTypeInternal,
// token 237,
ves_icall_System_Array_CanChangePrimitive,
// token 238,
ves_icall_System_Array_FastCopy,
// token 239,
ves_icall_System_Array_GetLengthInternal_raw,
// token 240,
ves_icall_System_Array_GetLowerBoundInternal_raw,
// token 241,
ves_icall_System_Array_GetGenericValue_icall,
// token 242,
ves_icall_System_Array_GetValueImpl_raw,
// token 243,
ves_icall_System_Array_SetGenericValue_icall,
// token 246,
ves_icall_System_Array_SetValueImpl_raw,
// token 247,
ves_icall_System_Array_InitializeInternal_raw,
// token 248,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 419,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 420,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 421,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 450,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 451,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 452,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 472,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 473,
ves_icall_System_Enum_InternalBoxEnum_raw,
// token 474,
ves_icall_System_Enum_InternalGetCorElementType,
// token 475,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 592,
ves_icall_System_Environment_get_ProcessorCount,
// token 593,
ves_icall_System_Environment_get_TickCount,
// token 594,
ves_icall_System_Environment_get_TickCount64,
// token 595,
ves_icall_System_Environment_Exit,
// token 598,
ves_icall_System_Environment_FailFast_raw,
// token 634,
ves_icall_System_GC_GetCollectionCount,
// token 635,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 636,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 639,
ves_icall_System_GC_SuppressFinalize_raw,
// token 641,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 643,
ves_icall_System_GC_GetGCMemoryInfo,
// token 645,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 650,
ves_icall_System_Object_MemberwiseClone_raw,
// token 658,
ves_icall_System_Math_Acos,
// token 659,
ves_icall_System_Math_Acosh,
// token 660,
ves_icall_System_Math_Asin,
// token 661,
ves_icall_System_Math_Asinh,
// token 662,
ves_icall_System_Math_Atan,
// token 663,
ves_icall_System_Math_Atan2,
// token 664,
ves_icall_System_Math_Atanh,
// token 665,
ves_icall_System_Math_Cbrt,
// token 666,
ves_icall_System_Math_Ceiling,
// token 667,
ves_icall_System_Math_Cos,
// token 668,
ves_icall_System_Math_Cosh,
// token 669,
ves_icall_System_Math_Exp,
// token 670,
ves_icall_System_Math_Floor,
// token 671,
ves_icall_System_Math_Log,
// token 672,
ves_icall_System_Math_Log10,
// token 673,
ves_icall_System_Math_Pow,
// token 674,
ves_icall_System_Math_Sin,
// token 676,
ves_icall_System_Math_Sinh,
// token 677,
ves_icall_System_Math_Sqrt,
// token 678,
ves_icall_System_Math_Tan,
// token 679,
ves_icall_System_Math_Tanh,
// token 680,
ves_icall_System_Math_FusedMultiplyAdd,
// token 681,
ves_icall_System_Math_Log2,
// token 682,
ves_icall_System_Math_ModF,
// token 774,
ves_icall_System_MathF_Acos,
// token 775,
ves_icall_System_MathF_Acosh,
// token 776,
ves_icall_System_MathF_Asin,
// token 777,
ves_icall_System_MathF_Asinh,
// token 778,
ves_icall_System_MathF_Atan,
// token 779,
ves_icall_System_MathF_Atan2,
// token 780,
ves_icall_System_MathF_Atanh,
// token 781,
ves_icall_System_MathF_Cbrt,
// token 782,
ves_icall_System_MathF_Ceiling,
// token 783,
ves_icall_System_MathF_Cos,
// token 784,
ves_icall_System_MathF_Cosh,
// token 785,
ves_icall_System_MathF_Exp,
// token 786,
ves_icall_System_MathF_Floor,
// token 787,
ves_icall_System_MathF_Log,
// token 788,
ves_icall_System_MathF_Log10,
// token 789,
ves_icall_System_MathF_Pow,
// token 790,
ves_icall_System_MathF_Sin,
// token 792,
ves_icall_System_MathF_Sinh,
// token 793,
ves_icall_System_MathF_Sqrt,
// token 794,
ves_icall_System_MathF_Tan,
// token 795,
ves_icall_System_MathF_Tanh,
// token 796,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 797,
ves_icall_System_MathF_Log2,
// token 798,
ves_icall_System_MathF_ModF,
// token 865,
ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw,
// token 866,
ves_icall_RuntimeMethodHandle_ReboxToNullable_raw,
// token 933,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 940,
ves_icall_RuntimeType_make_array_type_raw,
// token 943,
ves_icall_RuntimeType_make_byref_type_raw,
// token 945,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 950,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 951,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 953,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 954,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 958,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 959,
ves_icall_System_RuntimeType_AllocateValueType_raw,
// token 961,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 963,
ves_icall_System_RuntimeType_getFullName_raw,
// token 964,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 967,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 968,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 969,
ves_icall_RuntimeType_GetFields_native_raw,
// token 972,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 974,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 977,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 979,
ves_icall_RuntimeType_GetName_raw,
// token 981,
ves_icall_RuntimeType_GetNamespace_raw,
// token 990,
ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw,
// token 1057,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 1059,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 1061,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1071,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 1072,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 1073,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1074,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1076,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1083,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1084,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1085,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1086,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1087,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1095,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1096,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 1097,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1101,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 1102,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1104,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1108,
ves_icall_System_String_FastAllocateString_raw,
// token 1109,
ves_icall_System_String_InternalIsInterned_raw,
// token 1110,
ves_icall_System_String_InternalIntern_raw,
// token 1394,
ves_icall_System_Type_internal_from_handle_raw,
// token 1585,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1586,
ves_icall_System_ValueType_Equals_raw,
// token 10337,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 10338,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 10340,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 10341,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 10342,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 10343,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 10344,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 10346,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 10348,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 10350,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 10351,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 10362,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 10364,
mono_monitor_exit_icall_raw,
// token 10369,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 10371,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 10373,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 10375,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 10426,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 10427,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 10429,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 10430,
ves_icall_System_Threading_Thread_GetState_raw,
// token 10431,
ves_icall_System_Threading_Thread_SetState_raw,
// token 10432,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 10433,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 10435,
ves_icall_System_Threading_Thread_YieldInternal,
// token 10437,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 11529,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 11533,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 11535,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 11536,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 11537,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 11538,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 11856,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 11857,
ves_icall_System_GCHandle_InternalFree_raw,
// token 11858,
ves_icall_System_GCHandle_InternalGet_raw,
// token 11859,
ves_icall_System_GCHandle_InternalSet_raw,
// token 11879,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 11880,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 11881,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 11883,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 11931,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 12000,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw,
// token 12002,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw,
// token 12004,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 12013,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 12014,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 12015,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw,
// token 12016,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 12017,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 12466,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 12467,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 12472,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 12473,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 12506,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 12526,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 12533,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 12540,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 12551,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 12554,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 12579,
ves_icall_System_Reflection_LoaderAllocatorScout_Destroy,
// token 12661,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 12672,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 12674,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 12675,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 12676,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 12683,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 12697,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 12717,
ves_icall_reflection_get_token_raw,
// token 12718,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 12726,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 12728,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 12735,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 12736,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 12739,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 12741,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 12746,
ves_icall_reflection_get_token_raw,
// token 12754,
ves_icall_get_method_info_raw,
// token 12755,
ves_icall_get_method_attributes,
// token 12762,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 12764,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 12775,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodBodyInternal_raw,
// token 12778,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 12781,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 12782,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 12783,
ves_icall_reflection_get_token_raw,
// token 12794,
ves_icall_InternalInvoke_raw,
// token 12803,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 12809,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 12810,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 12811,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 12813,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 12814,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 12832,
ves_icall_InvokeClassConstructor_raw,
// token 12834,
ves_icall_InternalInvoke_raw,
// token 12849,
ves_icall_reflection_get_token_raw,
// token 12870,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 12871,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 12896,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 12901,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 12931,
ves_icall_reflection_get_token_raw,
// token 12932,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 13446,
ves_icall_AssemblyExtensions_ApplyUpdateEnabled,
// token 13447,
ves_icall_AssemblyExtensions_GetApplyUpdateCapabilities_raw,
// token 13474,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 13488,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 13582,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 13583,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 13797,
ves_icall_ModuleBuilder_basic_init_raw,
// token 13798,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 13805,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 13806,
ves_icall_ModuleBuilder_getToken_raw,
// token 13807,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 13813,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 13910,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 14391,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 14392,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 14749,
ves_icall_System_Diagnostics_Debugger_Log,
// token 14754,
ves_icall_System_Diagnostics_StackFrame_GetFrameInfo,
// token 14764,
ves_icall_System_Diagnostics_StackTrace_GetTrace,
// token 16259,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 16280,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 16282,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 16284,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_flags [] = {
0,
0,
0,
0,
0,
4,
4,
0,
4,
0,
4,
4,
4,
0,
0,
0,
4,
4,
4,
4,
4,
0,
4,
0,
0,
0,
0,
4,
0,
4,
4,
4,
4,
0,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
};
