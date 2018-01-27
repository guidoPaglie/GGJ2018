using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TelephoneCentral : MonoBehaviour
{
    public Board board;

    private List<PhoneCall> phoneCalls = new List<PhoneCall>();
    private float phoneCallRate;
    private float phoneCallRateTimer = 1;

    private int phoneCallIndex;
    private PhoneCall currentPhoneCall;

    public void InitializeRound(IEnumerable<PhoneCall> roundPhoneCalls, float roundPhoneCallRate)
    {
        phoneCallRate = roundPhoneCallRate;
        phoneCalls.Clear();

        phoneCalls.AddRange(roundPhoneCalls);
    }

    public void ConnectCall(int receptorId)
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
            }
        }
    }

    public void NotifyIncomingCall(PhoneCall phoneCall)
    {
        board.IncomingCall(phoneCall.caller);
    }

    private void Start()
    {
        board.SubscribeToReceptorEvent(ConnectCall);

        InitializeRound(new List<PhoneCall> { new PhoneCall(1, 2), new PhoneCall(3, 4) }, 2);
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