using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TelephoneCentral : MonoBehaviour
{
    public Board board;

    private List<PhoneCall> phoneCalls;
    private float phoneCallRate;
    private float phoneCallRateTimer;

    private int phoneCallIndex;
    private PhoneCall currentPhoneCall;

    public void InitializeRound(IEnumerable<PhoneCall> roundPhoneCalls, float roundPhoneCallRate)
    {
        phoneCallRate = roundPhoneCallRate;
        phoneCalls.Clear();

        phoneCalls.AddRange(roundPhoneCalls);
    }

    public bool ConnectCall(int receptorId)
    {
        if (phoneCalls.Count(x => !x.connected) > 0)
        {
            if (currentPhoneCall == null)
            {
                currentPhoneCall = phoneCalls.FirstOrDefault(x => x.caller == receptorId);
            }
            else if (currentPhoneCall.receiver == receptorId)
            {
                currentPhoneCall.connected = true;
                board.CallCompleted(currentPhoneCall.caller, currentPhoneCall.receiver);
                currentPhoneCall = null;
                return true;
            }

            return false;
        }

        return true;
    }

    public void NotifyIncomingCall(PhoneCall phoneCall)
    {
        board.IncomingCall(phoneCall.caller);
    }

    private void Update()
    {
        if (phoneCallIndex < phoneCalls.Count)
        {
            phoneCallRateTimer -= Time.deltaTime;

            if (phoneCallRateTimer <= 0)
            {
                NotifyIncomingCall(phoneCalls[phoneCallIndex]);

                phoneCallIndex++;

                phoneCallRateTimer = phoneCallRate;
            } 
        }

        if (phoneCalls.Count(x => x.connected) == phoneCalls.Count)
        {
            //board.NotifyEndOfRound();
        }
    }
}

/*
 * Generar llamadas
 * Enviar las llamadas a FE
 * Recivir conecciones
 * Verificar si estan correctamente conectadas
 * Responder como fue la coneccion
 * 
*/

public class PhoneCall
{
    public PhoneCall(int c, int r)
    {
        caller = c;
        receiver = r;
    }

    public int caller;
    public int receiver;

    public bool connected;
}