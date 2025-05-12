using System;
using System.Collections.Generic;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public enum Pembayaran
    {
        Tunai = 1,
        Bca,
        Bni,
        Mandiri,
        Bri,
        ShopeePay,
        Gopay,
        Dana
    }

    public class PembayaranInfo
    {
        public string Deskripsi { get; set; }
        public decimal BiayaAdmin { get; set; }
        public decimal MinimalPenarikan { get; set; }

        public PembayaranInfo(string deskripsi, decimal biayaAdmin, decimal minimalPenarikan)
        {
            Deskripsi = deskripsi;
            BiayaAdmin = biayaAdmin;
            MinimalPenarikan = minimalPenarikan;
        }
    }

    public class StateBasedPenarikan
    {
        public enum PenarikanState
        {
            MEMASUKKAN_DATA,
            VALIDASI,
            BERHASIL,
            GAGAL,
            MENUNGGU_APPROVAL,
            DITERIMA,
            DITOLAK
        }

        public enum PenarikanTrigger
        {
            SUBMIT,
            VALID,
            INVALID,
            RETRY,
            APPROVE,
            REJECT,
            TRANSFER
        }

        public class Transition
        {
            public PenarikanState prevState;
            public PenarikanState nextState;
            public PenarikanTrigger trigger;

            public Transition(PenarikanState prev, PenarikanState next, PenarikanTrigger trig)
            {
                this.prevState = prev;
                this.nextState = next;
                this.trigger = trig;
            }
        }

        public static readonly Dictionary<PenarikanState, Dictionary<PenarikanTrigger, PenarikanState>> stateTransitionTable = new Dictionary<PenarikanState, Dictionary<PenarikanTrigger, PenarikanState>>
        {
            {
                PenarikanState.MEMASUKKAN_DATA, new Dictionary<PenarikanTrigger, PenarikanState>
                {
                    { PenarikanTrigger.SUBMIT, PenarikanState.MENUNGGU_APPROVAL },
                    { PenarikanTrigger.INVALID, PenarikanState.GAGAL }
                }
            },
            {
                PenarikanState.VALIDASI, new Dictionary<PenarikanTrigger, PenarikanState>
                {
                    { PenarikanTrigger.VALID, PenarikanState.BERHASIL },
                    { PenarikanTrigger.INVALID, PenarikanState.GAGAL }
                }
            },
            {
                PenarikanState.GAGAL, new Dictionary<PenarikanTrigger, PenarikanState>
                {
                    { PenarikanTrigger.RETRY, PenarikanState.MEMASUKKAN_DATA }
                }
            },
            {
                PenarikanState.MENUNGGU_APPROVAL, new Dictionary<PenarikanTrigger, PenarikanState>
                {
                    { PenarikanTrigger.APPROVE, PenarikanState.DITERIMA },
                    { PenarikanTrigger.REJECT, PenarikanState.DITOLAK }
                }
            },
            {
                PenarikanState.DITERIMA, new Dictionary<PenarikanTrigger, PenarikanState>
                {
                    { PenarikanTrigger.TRANSFER, PenarikanState.BERHASIL }
                }
            },
            {
                PenarikanState.DITOLAK, new Dictionary<PenarikanTrigger, PenarikanState>
                {
                    { PenarikanTrigger.RETRY, PenarikanState.MEMASUKKAN_DATA }
                }
            }
        };

        public static PenarikanState GetNextState(PenarikanState prevState, PenarikanTrigger trigger)
        {
            if (stateTransitionTable.ContainsKey(prevState) && stateTransitionTable[prevState].ContainsKey(trigger))
            {
                return stateTransitionTable[prevState][trigger];
            }
            return prevState; 
        }
    }
}