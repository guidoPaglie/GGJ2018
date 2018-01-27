using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

    public Button SoundBtn;
    public Button MusicBtn;

    public Image SoundImg;
    public Sprite SoundOff;
    public Sprite SoundOn;

    public Image MusicImg;
    public Sprite MusicOff;
    public Sprite MusicOn;

    private bool _soundOn;
    private bool _musicOn;

	void Start () {
        _soundOn = PlayerPrefs.GetInt("sound", 0) == 1;
        _musicOn = PlayerPrefs.GetInt("music", 0) == 1;

        SoundBtn.onClick.AddListener(SwitchSoundState);
        MusicBtn.onClick.AddListener(SwitchMusicState);
	}

    private void SwitchSoundState()
    {
        _soundOn = !_soundOn;
        SoundImg.sprite = _soundOn ? SoundOn : SoundOff;

        PlayerPrefs.SetInt("sound", Convert.ToInt32(_soundOn));
    }

    private void SwitchMusicState()
    {
        _musicOn = !_musicOn;
        MusicImg.sprite = _musicOn ? MusicOn : MusicOff;

        PlayerPrefs.SetInt("music", Convert.ToInt32(_musicOn));
    }
}
