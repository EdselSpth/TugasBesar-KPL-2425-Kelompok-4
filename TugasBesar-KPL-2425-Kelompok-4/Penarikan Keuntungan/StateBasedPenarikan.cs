using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public class StateBasedPenarikan
    {
        // Define states for Penarikan
        public enum PenarikanState
        {
            MEMASUKKAN_DATA,    // Customer memasukkan data
            VALIDASI,           // Validasi data
            BERHASIL,           // Penarikan berhasil
            GAGAL,              // Penarikan gagal
            MENUNGGU_APPROVAL,  // Admin menyetujui
            DITERIMA,           // Admin menyetujui dan transfer
            DITOLAK             // Admin menolak
        }

        public enum PenarikanTrigger
        {
            SUBMIT,      // Customer submit data
            VALID,       // Data valid
            INVALID,     // Data tidak valid
            RETRY,       // Retry untuk Customer
            APPROVE,     // Admin approve
            REJECT,      // Admin reject
            TRANSFER     // Admin transfer
        }

        // Transition model between states
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

        // State transitions
        public static Transition[] transitions =
        {
            new Transition(PenarikanState.MEMASUKKAN_DATA, PenarikanState.VALIDASI, PenarikanTrigger.SUBMIT),
            new Transition(PenarikanState.VALIDASI, PenarikanState.BERHASIL, PenarikanTrigger.VALID),
            new Transition(PenarikanState.VALIDASI, PenarikanState.GAGAL, PenarikanTrigger.INVALID),
            new Transition(PenarikanState.GAGAL, PenarikanState.MEMASUKKAN_DATA, PenarikanTrigger.RETRY),
            new Transition(PenarikanState.MEMASUKKAN_DATA, PenarikanState.MENUNGGU_APPROVAL, PenarikanTrigger.SUBMIT),
            new Transition(PenarikanState.MENUNGGU_APPROVAL, PenarikanState.DITERIMA, PenarikanTrigger.APPROVE),
            new Transition(PenarikanState.MENUNGGU_APPROVAL, PenarikanState.DITOLAK, PenarikanTrigger.REJECT),
            new Transition(PenarikanState.DITERIMA, PenarikanState.BERHASIL, PenarikanTrigger.TRANSFER)
        };

        // Function to get the next state
        public static PenarikanState GetNextState(PenarikanState prevState, PenarikanTrigger trigger)
        {
            foreach (var t in transitions)
            {
                if (t.prevState == prevState && t.trigger == trigger)
                    return t.nextState;
            }
            return prevState;
        }
    }
}
