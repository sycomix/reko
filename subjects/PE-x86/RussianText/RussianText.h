// RussianText.h
// Generated by decompiling RussianText.exe
// using Decompiler version 0.6.0.0.

/*
// Equivalence classes ////////////
Eq_1: (struct "Globals" (40208B ui32 dw40208B) (4020F8 word32 dw4020F8))
	globals_t (in globals : (ptr (struct "Globals")))
Eq_3: (fn word32 ())
	T_3 (in fn00401158 : ptr32)
	T_4 (in signature of fn00401158 : void)
Eq_11: (struct "Eq_11" (0 word32 dw0000) (4 ptr32 ptr0004))
	T_11 (in dwArg04 : word32)
Eq_20: (segment "Eq_20" (2C (ptr (arr word32)) ptr002C))
	T_20 (in fs : selector)
// Type Variables ////////////
globals_t: (in globals : (ptr (struct "Globals")))
  Class: Eq_1
  DataType: (ptr Eq_1)
  OrigDataType: (ptr (struct "Globals"))
T_2: (in dwArg04 : word32)
  Class: Eq_2
  DataType: word32
  OrigDataType: word32
T_3: (in fn00401158 : ptr32)
  Class: Eq_3
  DataType: (ptr Eq_3)
  OrigDataType: (ptr (fn T_5 ()))
T_4: (in signature of fn00401158 : void)
  Class: Eq_3
  DataType: (ptr Eq_3)
  OrigDataType: 
T_5: (in fn00401158() : word32)
  Class: Eq_5
  DataType: word32
  OrigDataType: word32
T_6: (in 0x0000001C : word32)
  Class: Eq_6
  DataType: word32
  OrigDataType: word32
T_7: (in fn00401158() + 0x0000001C : word32)
  Class: Eq_7
  DataType: word32
  OrigDataType: word32
T_8: (in 0x004020F8 : ptr32)
  Class: Eq_8
  DataType: (ptr word32)
  OrigDataType: (ptr (struct (0 T_9 t0000)))
T_9: (in Mem15[0x004020F8:word32] : word32)
  Class: Eq_7
  DataType: word32
  OrigDataType: word32
T_10: (in 0x82727349 : word32)
  Class: Eq_10
  DataType: word32
  OrigDataType: word32
T_11: (in dwArg04 : word32)
  Class: Eq_11
  DataType: (ptr Eq_11)
  OrigDataType: (ptr (struct (0 T_14 t0000) (4 T_18 t0004)))
T_12: (in 0x00000000 : word32)
  Class: Eq_12
  DataType: word32
  OrigDataType: word32
T_13: (in dwArg04 + 0x00000000 : word32)
  Class: Eq_13
  DataType: word32
  OrigDataType: word32
T_14: (in Mem16[dwArg04 + 0x00000000:word32] : word32)
  Class: Eq_10
  DataType: word32
  OrigDataType: word32
T_15: (in 0x004020E4 : ptr32)
  Class: Eq_15
  DataType: ptr32
  OrigDataType: ptr32
T_16: (in 0x00000004 : word32)
  Class: Eq_16
  DataType: word32
  OrigDataType: word32
T_17: (in dwArg04 + 0x00000004 : word32)
  Class: Eq_17
  DataType: ptr32
  OrigDataType: ptr32
T_18: (in Mem17[dwArg04 + 0x00000004:word32] : word32)
  Class: Eq_15
  DataType: ptr32
  OrigDataType: word32
T_19: (in eax : word32)
  Class: Eq_19
  DataType: word32
  OrigDataType: word32
T_20: (in fs : selector)
  Class: Eq_20
  DataType: (ptr Eq_20)
  OrigDataType: (ptr (segment (2C T_22 t002C)))
T_21: (in 0x002C : word16)
  Class: Eq_21
  DataType: (memptr (ptr Eq_20) (ptr (arr word32)))
  OrigDataType: (memptr T_20 (struct (0 T_22 t0000)))
T_22: (in Mem0[fs:0x002C:word32] : word32)
  Class: Eq_22
  DataType: (ptr (arr word32))
  OrigDataType: (ptr (struct (0 (arr T_40) a0000)))
T_23: (in 0x0040208B : ptr32)
  Class: Eq_23
  DataType: (ptr ui32)
  OrigDataType: (ptr (struct (0 T_24 t0000)))
T_24: (in Mem0[0x0040208B:word32] : word32)
  Class: Eq_24
  DataType: ui32
  OrigDataType: ui32
T_25: (in 0x00000004 : word32)
  Class: Eq_25
  DataType: ui32
  OrigDataType: ui32
T_26: (in globals->dw40208B * 0x00000004 : word32)
  Class: Eq_26
  DataType: ui32
  OrigDataType: ui32
T_27: (in Mem0[fs:0x002C:word32][Mem0[0x0040208B:word32] * 0x00000004] : word32)
  Class: Eq_19
  DataType: word32
  OrigDataType: word32
T_28: (in eax : int32)
  Class: Eq_28
  DataType: int32
  OrigDataType: int32
T_29: (in argc : int32)
  Class: Eq_29
  DataType: int32
  OrigDataType: int32
T_30: (in argv : (ptr (ptr char)))
  Class: Eq_30
  DataType: (ptr (ptr char))
  OrigDataType: (ptr (ptr char))
T_31: (in esp_13 : word32)
  Class: Eq_31
  DataType: word32
  OrigDataType: word32
T_32: (in ebp_14 : word32)
  Class: Eq_32
  DataType: word32
  OrigDataType: word32
T_33: (in ecx_15 : word32)
  Class: Eq_33
  DataType: word32
  OrigDataType: word32
T_34: (in eax_16 : word32)
  Class: Eq_34
  DataType: word32
  OrigDataType: word32
T_35: (in SZO_17 : byte)
  Class: Eq_35
  DataType: byte
  OrigDataType: byte
T_36: (in C_18 : byte)
  Class: Eq_36
  DataType: byte
  OrigDataType: byte
T_37: (in printf : ptr32)
  Class: Eq_37
  DataType: (ptr code)
  OrigDataType: (ptr code)
T_38: (in signature of printf : void)
  Class: Eq_37
  DataType: (ptr code)
  OrigDataType: 
T_39: (in 0x00000000 : word32)
  Class: Eq_39
  DataType: word32
  OrigDataType: word32
T_40:
  Class: Eq_40
  DataType: word32
  OrigDataType: (struct 0004 (0 T_27 t0000))
*/
typedef struct Globals {
	ui32 dw40208B;	// 40208B
	word32 dw4020F8;	// 4020F8
} Eq_1;

typedef word32 (Eq_3)();

typedef struct Eq_11 {
	word32 dw0000;	// 0
	ptr32 ptr0004;	// 4
} Eq_11;

typedef struct Eq_20 {
	word32 ( * ptr002C)[];	// 2C
} Eq_20;

