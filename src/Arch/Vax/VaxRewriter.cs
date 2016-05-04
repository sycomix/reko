﻿#region License
/* 
 * Copyright (C) 1999-2016 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Reko.Core;
using Reko.Core.Rtl;
using Reko.Core.Expressions;
using Reko.Core.Types;
using System.Diagnostics;
using System.Linq;
using Reko.Core.Machine;
using Reko.Core.Operators;

namespace Reko.Arch.Vax
{
    public partial class VaxRewriter : IEnumerable<RtlInstructionCluster>
    {
        private Frame frame;
        private IRewriterHost host;
        private ImageReader rdr;
        private ProcessorState state;
        private VaxArchitecture arch;
        private RtlInstructionCluster rtlc;
        private RtlEmitter emitter;
        private IEnumerator<VaxInstruction> dasm;

        public VaxRewriter(VaxArchitecture arch, ImageReader rdr, ProcessorState state, Frame frame, IRewriterHost host)
        {
            this.arch = arch;
            this.rdr = rdr;
            this.state = state;
            this.frame = frame;
            this.host = host;
            this.dasm = new VaxDisassembler(arch, rdr).GetEnumerator();
        }

        public IEnumerator<RtlInstructionCluster> GetEnumerator()
        {
            while (dasm.MoveNext())
            {
                rtlc = new RtlInstructionCluster(dasm.Current.Address, dasm.Current.Length);
                rtlc.Class = RtlClass.Linear;
                emitter = new RtlEmitter(rtlc.Instructions);
                switch (dasm.Current.Opcode)
                {
                default:
                    //EmitUnitTest();
                    //emitter.SideEffect(Constant.String(
                    //    dasm.Current.ToString(),
                    //    StringType.NullTerminated(PrimitiveType.Char)));
                    //break;
                    throw new AddressCorrelatedException(
                        dasm.Current.Address,
                        "Rewriting of VAX instruction {0} not implemented yet.",
                        dasm.Current.Opcode);
                case Opcode.addb2: RewriteAlu2(PrimitiveType.Byte, emitter.IAdd, AllFlags); break;
                case Opcode.addb3: RewriteAlu3(PrimitiveType.Byte, emitter.IAdd, AllFlags); break;
                case Opcode.addd2: RewriteFpu2(PrimitiveType.Real64, emitter.FAdd, NZ00); break;
                case Opcode.addd3: RewriteFpu3(PrimitiveType.Real64, emitter.FAdd, NZ00); break;
                case Opcode.addf2: RewriteFpu2(PrimitiveType.Real32, emitter.FAdd, NZ00); break;
                case Opcode.addf3: RewriteFpu3(PrimitiveType.Real32, emitter.FAdd, NZ00); break;
                case Opcode.addl2: RewriteAlu2(PrimitiveType.Word32, emitter.IAdd, AllFlags); break;
                case Opcode.addl3: RewriteAlu3(PrimitiveType.Word32, emitter.IAdd, AllFlags); break;
                case Opcode.addp4: RewriteAddp4(); break;
                case Opcode.addp6: RewriteAddp6(); break;
                case Opcode.addw2: RewriteAlu2(PrimitiveType.Word16, emitter.IAdd, AllFlags); break;
                case Opcode.addw3: RewriteAlu3(PrimitiveType.Word16, emitter.IAdd, AllFlags); break;
                case Opcode.adwc: RewriteAdwc(); break;
                case Opcode.aobleq: RewriteAobleq(); break;
                case Opcode.ashl: RewriteAsh(PrimitiveType.Word32); break;
                case Opcode.ashp: RewriteAshp(); break;
                case Opcode.ashq: RewriteAsh(PrimitiveType.Word64); break;
                case Opcode.bbc: RewriteBb(false); break;
                case Opcode.bbs: RewriteBb(true); break;
                case Opcode.beql: RewriteBranch(ConditionCode.EQ, FlagM.ZF); break;
                case Opcode.bgeq: RewriteBranch(ConditionCode.GE, FlagM.NF); break;
                case Opcode.bgequ: RewriteBranch(ConditionCode.UGE, FlagM.CF); break;
                case Opcode.bgtr: RewriteBranch(ConditionCode.GT, FlagM.ZF | FlagM.NF); break;
                case Opcode.bgtru: RewriteBranch(ConditionCode.GT, FlagM.ZF | FlagM.CF); break;
                case Opcode.bleq: RewriteBranch(ConditionCode.LE, FlagM.ZF | FlagM.NF); break;
                case Opcode.bneq: RewriteBranch(ConditionCode.NE, FlagM.ZF); break;
                case Opcode.bicb2: RewriteAlu2(PrimitiveType.Byte, Bic, NZ00); break;
                case Opcode.bicb3: RewriteAlu3(PrimitiveType.Byte, Bic, NZ00); break;
                case Opcode.bicl2: RewriteAlu2(PrimitiveType.Word32, Bic, NZ00); break;
                case Opcode.bicl3: RewriteAlu3(PrimitiveType.Word32, Bic, NZ00); break;
                case Opcode.bicw2: RewriteAlu2(PrimitiveType.Word16, Bic, NZ00); break;
                case Opcode.bicw3: RewriteAlu3(PrimitiveType.Word16, Bic, NZ00); break;

                case Opcode.halt: RewriteHalt(); break;
                case Opcode.bsbw: goto default;
                case Opcode.nop: goto default;
                case Opcode.brw: goto default;
                case Opcode.rei: goto default;
                case Opcode.cvtwl: goto default;
                case Opcode.bpt: goto default;
                case Opcode.cvtwb: goto default;
                case Opcode.ret: goto default;
                case Opcode.movp: goto default;
                case Opcode.rsb: goto default;
                case Opcode.cmpp3: goto default;
                case Opcode.ldpctx: goto default;
                case Opcode.cvtpl: goto default;
                case Opcode.svpctx: goto default;
                case Opcode.cmpp4: goto default;
                case Opcode.cvtps: goto default;
                case Opcode.editpc: goto default;
                case Opcode.cvtsp: goto default;
                case Opcode.matchc: goto default;
                case Opcode.index: goto default;
                case Opcode.locc: goto default;
                case Opcode.crc: goto default;
                case Opcode.skpc: goto default;
                case Opcode.prober: goto default;
                case Opcode.movzwl: goto default;
                case Opcode.probew: goto default;
                case Opcode.acbw: goto default;
                case Opcode.insque: goto default;
                case Opcode.movaw: goto default;
                case Opcode.remque: goto default;
                case Opcode.pushaw: goto default;
                case Opcode.bsbb: goto default;
                case Opcode.brb: goto default;
                case Opcode.subf2: goto default;
                case Opcode.subf3: goto default;
                case Opcode.mulf2: goto default;
                case Opcode.mulf3: goto default;
                case Opcode.jsb: goto default;
                case Opcode.divf2: goto default;
                case Opcode.jmp: goto default;
                case Opcode.divf3: goto default;
                case Opcode.cvtfb: goto default;
                case Opcode.blss: goto default;
                case Opcode.cvtfw: goto default;
                case Opcode.cvtfl: goto default;
                case Opcode.blequ: goto default;
                case Opcode.cvtrfl: goto default;
                case Opcode.bvc: goto default;
                case Opcode.cvtbf: goto default;
                case Opcode.bvs: goto default;
                case Opcode.cvtwf: goto default;
                case Opcode.cvtlf: goto default;
                case Opcode.blssu: goto default;
                case Opcode.acbf: goto default;
                case Opcode.movf: goto default;
                case Opcode.cmpf: goto default;
                case Opcode.subp4: goto default;
                case Opcode.mnegf: goto default;
                case Opcode.subp6: goto default;
                case Opcode.tstf: goto default;
                case Opcode.cvtpt: goto default;
                case Opcode.emodf: goto default;
                case Opcode.mulp: goto default;
                case Opcode.polyf: goto default;
                case Opcode.cvttp: goto default;
                case Opcode.cvtfd: goto default;
                case Opcode.divp: goto default;
                case Opcode.movc3: goto default;
                case Opcode.adawi: goto default;
                case Opcode.cmpc3: goto default;
                case Opcode.scanc: goto default;
                case Opcode.spanc: goto default;
                case Opcode.movc5: goto default;
                case Opcode.insqhi: goto default;
                case Opcode.cmpc5: goto default;
                case Opcode.insqti: goto default;
                case Opcode.movtc: goto default;
                case Opcode.remqhi: goto default;
                case Opcode.movtuc: goto default;
                case Opcode.remqti: goto default;
                case Opcode.movb: goto default;
                case Opcode.cmpb: goto default;
                case Opcode.subd2: goto default;
                case Opcode.mcomb: goto default;
                case Opcode.subd3: goto default;
                case Opcode.bitb: goto default;
                case Opcode.muld2: goto default;
                case Opcode.clrb: goto default;
                case Opcode.muld3: goto default;
                case Opcode.tstb: goto default;
                case Opcode.divd2: goto default;
                case Opcode.incb: goto default;
                case Opcode.divd3: goto default;
                case Opcode.decb: goto default;
                case Opcode.cvtdb: goto default;
                case Opcode.cvtbl: goto default;
                case Opcode.cvtdw: goto default;
                case Opcode.cvtbw: goto default;
                case Opcode.cvtdl: goto default;
                case Opcode.movzbl: goto default;
                case Opcode.cvtrdl: goto default;
                case Opcode.movzbw: goto default;
                case Opcode.cvtbd: goto default;
                case Opcode.rotl: goto default;
                case Opcode.cvtwd: goto default;
                case Opcode.acbb: goto default;
                case Opcode.cvtld: goto default;
                case Opcode.movab: goto default;
                case Opcode.acbd: goto default;
                case Opcode.pushab: goto default;
                case Opcode.movd: goto default;
                case Opcode.cmpd: goto default;
                case Opcode.mnegd: goto default;
                case Opcode.subw2: goto default;
                case Opcode.tstd: goto default;
                case Opcode.subw3: goto default;
                case Opcode.emodd: goto default;
                case Opcode.mulw2: goto default;
                case Opcode.polyd: goto default;
                case Opcode.mulw3: goto default;
                case Opcode.cvtdf: goto default;
                case Opcode.divw2: goto default;
                case Opcode.divw3: goto default;
                case Opcode.bisw2: goto default;
                case Opcode.bisw3: goto default;
                case Opcode.emul: goto default;
                case Opcode.ediv: goto default;
                case Opcode.clrq: goto default;
                case Opcode.xorw2: goto default;
                case Opcode.movq: goto default;
                case Opcode.xorw3: goto default;
                case Opcode.movaq: goto default;
                case Opcode.mnegw: goto default;
                case Opcode.pushaq: goto default;
                case Opcode.casew: goto default;
                case Opcode.movw: goto default;
                case Opcode.cmpw: goto default;
                case Opcode.subb2: goto default;
                case Opcode.mcomw: goto default;
                case Opcode.subb3: goto default;
                case Opcode.bitw: goto default;
                case Opcode.mulb2: goto default;
                case Opcode.clrw: goto default;
                case Opcode.mulb3: goto default;
                case Opcode.tstw: goto default;
                case Opcode.divb2: goto default;
                case Opcode.incw: goto default;
                case Opcode.divb3: goto default;
                case Opcode.decw: goto default;
                case Opcode.bisb2: goto default;
                case Opcode.bispsw: goto default;
                case Opcode.bisb3: goto default;
                case Opcode.bicpsw: goto default;
                case Opcode.popr: goto default;
                case Opcode.pushr: goto default;
                case Opcode.xorb2: goto default;
                case Opcode.chmk: goto default;
                case Opcode.xorb3: goto default;
                case Opcode.chme: goto default;
                case Opcode.mnegb: goto default;
                case Opcode.chms: goto default;
                case Opcode.caseb: goto default;
                case Opcode.chmu: goto default;
                case Opcode.subl2: goto default;
                case Opcode.bbss: goto default;
                case Opcode.subl3: goto default;
                case Opcode.bbcs: goto default;
                case Opcode.mull2: goto default;
                case Opcode.bbsc: goto default;
                case Opcode.mull3: goto default;
                case Opcode.bbcc: goto default;
                case Opcode.divl2: goto default;
                case Opcode.bbssi: goto default;
                case Opcode.divl3: goto default;
                case Opcode.bbcci: goto default;
                case Opcode.bisl2: goto default;
                case Opcode.blbs: goto default;
                case Opcode.bisl3: goto default;
                case Opcode.blbc: goto default;
                case Opcode.ffs: goto default;
                case Opcode.ffc: goto default;
                case Opcode.xorl2: goto default;
                case Opcode.cmpv: goto default;
                case Opcode.xorl3: goto default;
                case Opcode.cmpzv: goto default;
                case Opcode.mnegl: goto default;
                case Opcode.extv: goto default;
                case Opcode.casel: goto default;
                case Opcode.extzv: goto default;
                case Opcode.movl: goto default;
                case Opcode.insv: goto default;
                case Opcode.cmpl: goto default;
                case Opcode.acbl: goto default;
                case Opcode.mcoml: goto default;
                case Opcode.aoblss: goto default;
                case Opcode.bitl: goto default;
                case Opcode.clrl: goto default;
                case Opcode.sobgeq: goto default;
                case Opcode.tstl: goto default;
                case Opcode.sobgtr: goto default;
                case Opcode.incl: goto default;
                case Opcode.cvtlb: goto default;
                case Opcode.decl: goto default;
                case Opcode.cvtlw: goto default;
                case Opcode.sbwc: goto default;
                case Opcode.cvtlp: goto default;
                case Opcode.mtpr: goto default;
                case Opcode.callg: goto default;
                case Opcode.mfpr: goto default;
                case Opcode.calls: goto default;
                case Opcode.movpsl: goto default;
                case Opcode.xfc: goto default;
                case Opcode.pushl: goto default;
                case Opcode.moval: goto default;
                case Opcode.pushal: goto default;
                case Opcode.mfvp: goto default;
                case Opcode.cvtdh: goto default;
                case Opcode.cvtgf: goto default;
                case Opcode.vldl: goto default;
                case Opcode.vgathl: goto default;
                case Opcode.vldq: goto default;
                case Opcode.vgathq: goto default;
                case Opcode.addg2: goto default;
                case Opcode.addg3: goto default;
                case Opcode.subg2: goto default;
                case Opcode.subg3: goto default;
                case Opcode.mulg2: goto default;
                case Opcode.mulg3: goto default;
                case Opcode.divg2: goto default;
                case Opcode.divg3: goto default;
                case Opcode.cvtgb: goto default;
                case Opcode.cvtgw: goto default;
                case Opcode.cvtgl: goto default;
                case Opcode.cvtrgl: goto default;
                case Opcode.cvtbg: goto default;
                case Opcode.cvtwg: goto default;
                case Opcode.cvtlg: goto default;
                case Opcode.acbg: goto default;
                case Opcode.movg: goto default;
                case Opcode.cmpg: goto default;
                case Opcode.mnegg: goto default;
                case Opcode.tstg: goto default;
                case Opcode.emodg: goto default;
                case Opcode.polyg: goto default;
                case Opcode.cvtgh: goto default;
                case Opcode.addh2: goto default;
                case Opcode.addh3: goto default;
                case Opcode.subh2: goto default;
                case Opcode.subh3: goto default;
                case Opcode.mulh2: goto default;
                case Opcode.mulh3: goto default;
                case Opcode.divh2: goto default;
                case Opcode.divh3: goto default;
                case Opcode.cvthb: goto default;
                case Opcode.cvtfh: goto default;
                case Opcode.cvthw: goto default;
                case Opcode.cvtfg: goto default;
                case Opcode.cvthl: goto default;
                case Opcode.cvtrhl: goto default;
                case Opcode.cvtbh: goto default;
                case Opcode.vstl: goto default;
                case Opcode.cvtwh: goto default;
                case Opcode.vscatl: goto default;
                case Opcode.cvtlh: goto default;
                case Opcode.vstq: goto default;
                case Opcode.acbh: goto default;
                case Opcode.vscatq: goto default;
                case Opcode.movh: goto default;
                case Opcode.vvmull: goto default;
                case Opcode.cmph: goto default;
                case Opcode.vsmull: goto default;
                case Opcode.mnegh: goto default;
                case Opcode.vvmulg: goto default;
                case Opcode.tsth: goto default;
                case Opcode.vsmulg: goto default;
                case Opcode.emodh: goto default;
                case Opcode.vvmulf: goto default;
                case Opcode.polyh: goto default;
                case Opcode.vsmulf: goto default;
                case Opcode.cvthg: goto default;
                case Opcode.vvmuld: goto default;
                case Opcode.vsmuld: goto default;
                case Opcode.vsync: goto default;
                case Opcode.mtvp: goto default;
                case Opcode.vvdivg: goto default;
                case Opcode.vsdivg: goto default;
                case Opcode.clrh: goto default;
                case Opcode.vvdivf: goto default;
                case Opcode.movo: goto default;
                case Opcode.vsdivf: goto default;
                case Opcode.movah: goto default;
                case Opcode.vvdivd: goto default;
                case Opcode.pushah: goto default;
                case Opcode.vsdivd: goto default;
                case Opcode.vvaddl: goto default;
                case Opcode.vsaddl: goto default;
                case Opcode.vvaddg: goto default;
                case Opcode.vsaddg: goto default;
                case Opcode.vvaddf: goto default;
                case Opcode.vsaddf: goto default;
                case Opcode.vvaddd: goto default;
                case Opcode.vsaddd: goto default;
                case Opcode.vvsubl: goto default;
                case Opcode.vssubl: goto default;
                case Opcode.vvsubg: goto default;
                case Opcode.vssubg: goto default;
                case Opcode.vvsubf: goto default;
                case Opcode.vssubf: goto default;
                case Opcode.vvsubd: goto default;
                case Opcode.vssubd: goto default;
                case Opcode.vvcmpl: goto default;
                case Opcode.vvsrll: goto default;
                case Opcode.vscmpl: goto default;
                case Opcode.vssrll: goto default;
                case Opcode.vvcmpg: goto default;
                case Opcode.vscmpg: goto default;
                case Opcode.vvcmpf: goto default;
                case Opcode.vvslll: goto default;
                case Opcode.vscmpf: goto default;
                case Opcode.vsslll: goto default;
                case Opcode.vvcmpd: goto default;
                case Opcode.vscmpd: goto default;
                case Opcode.vvbisl: goto default;
                case Opcode.vvxorl: goto default;
                case Opcode.vsbisl: goto default;
                case Opcode.vsxorl: goto default;
                case Opcode.vvbicl: goto default;
                case Opcode.vvcvt: goto default;
                case Opcode.vsbicl: goto default;
                case Opcode.iota: goto default;
                case Opcode.vvmerge: goto default;
                case Opcode.vsmerge: goto default;
                case Opcode.cvthf: goto default;
                case Opcode.cvthd: goto default;
                case Opcode.bugl: goto default;
                case Opcode.bugw: goto default;
                }
                yield return rtlc;
            }
        }

        private static HashSet<Opcode> seen = new HashSet<Opcode>();

        [Conditional("DEBUG")]
        private void EmitUnitTest()
        {
            if (seen.Contains(dasm.Current.Opcode))
                return;
            seen.Add(dasm.Current.Opcode);

            var r2 = rdr.Clone();
            r2.Offset -= dasm.Current.Length;
            var bytes = r2.ReadBytes(dasm.Current.Length);
            Debug.WriteLine("        [Test]");
            Debug.WriteLine("        public void VaxRw_" + dasm.Current.Opcode + "()");
            Debug.WriteLine("        {");
            Debug.Write("            BuildTest(");
            Debug.Write(string.Join(
                ", ",
                bytes.Select(b => string.Format("0x{0:X2}", (int)b))));
            Debug.WriteLine(");\t// " + dasm.Current.ToString());
            Debug.WriteLine("            AssertCode(");
            Debug.WriteLine("                \"0|L--|00100000(2): 1 instructions\",");
            Debug.WriteLine("                \"1|L--|@@@\");");
            Debug.WriteLine("        }");
            Debug.WriteLine("");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //$REVIEW: push PseudoProc into the RewriterHost interface"
        public Expression PseudoProc(string name, DataType retType, params Expression[] args)
        {
            var ppp = host.EnsurePseudoProcedure(name, retType, args.Length);
            return PseudoProc(ppp, retType, args);
        }

        public Expression PseudoProc(PseudoProcedure ppp, DataType retType, params Expression[] args)
        {
            if (args.Length != ppp.Arity)
                throw new ArgumentOutOfRangeException(
                    string.Format("Pseudoprocedure {0} expected {1} arguments, but was passed {2}.",
                    ppp.Name,
                    ppp.Arity,
                    args.Length));

            return emitter.Fn(new ProcedureConstant(arch.PointerType, ppp), retType, args);
        }

        private Expression RewriteSrcOp(int iOp, PrimitiveType width)
        {
            var op = dasm.Current.Operands[iOp];
            var regOp = op as RegisterOperand;
            if (regOp != null)
            {
                var reg = frame.EnsureRegister(regOp.Register);
                if (reg.DataType.Size <= width.Size)
                {
                    return reg;
                } else
                {
                    return emitter.Cast(width, reg);
                }
            }
            var immOp = op as ImmediateOperand;
            if (immOp != null)
            {
                return immOp.Value;
            }
            var memOp = op as MemoryOperand;
            if (memOp != null)
            {
                Expression ea;
                if (memOp.Base != null)
                {
                    var reg = frame.EnsureRegister(memOp.Base);
                    if (memOp.AutoDecrement)
                    {
                        emitter.Assign(reg, emitter.ISub(reg, width.Size));
                    }
                    ea = reg;
                    if (memOp.Offset != null)
                    {
                        ea = emitter.IAdd(ea, memOp.Offset);
                    }
                    var mem = emitter.Load(width, ea);
                    if (memOp.AutoIncrement)
                    {
                        throw new NotImplementedException(op.GetType().Name);
                    }
                    return mem;
                }
                else
                {
                }
            }
            throw new NotImplementedException(op.GetType().Name);
        }

        private Identifier RewriteDstOp(int iOp, PrimitiveType width, Func<Expression, Expression> fn)
        {
            var op = dasm.Current.Operands[iOp];
            var regOp = op as RegisterOperand;
            if (regOp != null)
            {
                var reg = frame.EnsureRegister(regOp.Register);
                if (reg.DataType.Size > width.Size)
                {
                    var tmp = frame.CreateTemporary(width);
                    emitter.Assign(tmp, fn(emitter.Cast(width, reg)));
                    emitter.Assign(reg, emitter.Dpb(reg, tmp, 0));
                    return tmp;
                }
                else
                {
                    emitter.Assign(reg, fn(reg));
                    return reg;
                }
            }
            if( op is ImmediateOperand)
            {
                throw new AddressCorrelatedException(
                    dasm.Current.Address,
                    "Instruction {0} is attempting to assign to an immediate value.",
                    dasm.Current);
            }
            var memOp = op as MemoryOperand;
            if (memOp != null)
            {
                Expression ea;
                if (memOp.Base != null)
                {
                    var reg = frame.EnsureRegister(memOp.Base);
                    if (memOp.AutoDecrement)
                    {
                        emitter.Assign(reg, emitter.ISub(reg, width.Size));
                    }
                    ea = reg;
                    if (memOp.Offset != null)
                    {
                        ea = emitter.IAdd(ea, memOp.Offset);
                    }
                    var tmp = frame.CreateTemporary(width);
                    emitter.Assign(tmp, fn(emitter.Load(width, ea)));
                    emitter.Assign(emitter.Load(width, ea), tmp);

                    if (memOp.AutoIncrement)
                    {
                        throw new NotImplementedException(op.GetType().Name);
                    }
                    return tmp;
                }
                else
                {
                }
            }
            throw new NotImplementedException(op.GetType().Name);

        }

        private Identifier FlagGroup(FlagM flags)
        {
            return frame.EnsureFlagGroup(Registers.psw, (uint)flags, arch.GrfToString((uint)flags), PrimitiveType.Byte);
        }

        private void AllFlags(Expression dst)
        {
            var grf = FlagGroup(FlagM.NZVC);
            emitter.Assign(grf, emitter.Cond(dst));
        }

        private void NZ00(Expression dst)
        {
            var grf = FlagGroup(FlagM.NF| FlagM.ZF);
            emitter.Assign(grf, emitter.Cond(dst));
            var c = FlagGroup(FlagM.CF);
            var v = FlagGroup(FlagM.VF);
            emitter.Assign(c, Constant.False());
            emitter.Assign(v, Constant.False());
        }

        private void NZV0(Expression dst)
        {
            var grf = FlagGroup(FlagM.NF | FlagM.ZF | FlagM.VF);
            emitter.Assign(grf, emitter.Cond(dst));
            var c = FlagGroup(FlagM.CF);
            emitter.Assign(c, Constant.False());
        }
    }
}
