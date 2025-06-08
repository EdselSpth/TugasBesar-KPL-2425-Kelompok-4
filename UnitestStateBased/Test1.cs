using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;
using TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan;

namespace UnitestCodereuse
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestStateTransition_CorrectNextState()
        {
            // Tes code reuse dari state transition table
            var nextState = StateBasedPenarikan.GetNextState(PenarikanState.MEMASUKKAN_DATA, PenarikanTrigger.SUBMIT);
            Assert.AreEqual(PenarikanState.MENUNGGU_APPROVAL, nextState);

            nextState = StateBasedPenarikan.GetNextState(PenarikanState.MENUNGGU_APPROVAL, PenarikanTrigger.APPROVE);
            Assert.AreEqual(PenarikanState.TRANSFERSEDANGBERJALAN, nextState);

            nextState = StateBasedPenarikan.GetNextState(PenarikanState.TRANSFERSEDANGBERJALAN, PenarikanTrigger.TRANSFER);
            Assert.AreEqual(PenarikanState.BERHASIL, nextState);
        }

        [TestMethod]
        public void TestPenarikanAdmin_ApproveChangesStateToBerhasil()
        {
            PenarikanCustomer.RiwayatPenarikan.Clear();

            // Setup data penarikan customer
            var norek = "0987654321";
            decimal nominal = 100000;
            Pembayaran metode = Pembayaran.Bni;
            PenarikanCustomer.RiwayatPenarikan.Add(new PenarikanData(norek, nominal, metode));

            PenarikanState currentState = PenarikanState.MENUNGGU_APPROVAL;

            // Simulasi proses approve admin
            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.APPROVE);
            Assert.AreEqual(PenarikanState.TRANSFERSEDANGBERJALAN, currentState);

            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.TRANSFER);
            Assert.AreEqual(PenarikanState.BERHASIL, currentState);
        }

        [TestMethod]
        public void TestPenarikanAdmin_RejectChangesStateToDitolak()
        {
            PenarikanCustomer.RiwayatPenarikan.Clear();

            // Setup data penarikan customer
            var norek = "1122334455";
            decimal nominal = 75000;
            Pembayaran metode = Pembayaran.Mandiri;
            PenarikanCustomer.RiwayatPenarikan.Add(new PenarikanData(norek, nominal, metode));

            PenarikanState currentState = PenarikanState.MENUNGGU_APPROVAL;

            // Simulasi proses reject admin
            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.REJECT);

            Assert.AreEqual(PenarikanState.DITOLAK, currentState);
        }
    }
}
