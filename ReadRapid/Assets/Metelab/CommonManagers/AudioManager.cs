using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metelab.CommonManagers.AudioManager_;

namespace Metelab.CommonManagers
{
    public enum Audios
    {
        BackgroundMusicDefault = 0,
        ButtonClick = 1,
        Victory = 2,
        WrongMove = 3,
        ChestOpening = 4,
        TrueMove = 5,
        GoldColleting = 6,
        BoxSliding = 7,
        BoxFalling = 8
    }

    public class AudioManager : MeteSingleton<AudioManager>
    {
        public AudioManagerData AudioManagerData;

        [SerializeField] private List<AudioSource> ListDeactiveAudioSource = new();
        [SerializeField] private List<AudioSource> ListActiveAudioSource = new();
        [SerializeField] private Dictionary<Audios, AudioSource> DicLoopAudioSource = new();
        private int mAudioSourceCount;


        public override void EarlyInit()
        {
            base.EarlyInit();
            mAudioSourceCount = 0;
            AudioManagerData.EarlyInit();
        }

        public override void Init()
        {
            base.Init();
            AudioManagerData.Init();
            MeteButton.ActionOnGlobalClick += OnGlobalClick;
            PlayLoop(Audios.BackgroundMusicDefault);
        }

        private void OnDestroy()
        {
            MeteButton.ActionOnGlobalClick -= OnGlobalClick;
        }

        public void SetMuteActives(bool isMute)
        {
            foreach (var audioSource in ListActiveAudioSource)
            {
                if (audioSource != null)
                    audioSource.mute = isMute;
            }
        }

        public void SetMuteLoops(bool isMute)
        {
            foreach (KeyValuePair<Audios, AudioSource> item in DicLoopAudioSource)
            {
                if (item.Value != null)
                    item.Value.mute = isMute;
            }
        }

        public void PlayOneShot(Audios audio)
        {
            StartCoroutine(IPlayOneShot(audio));
        }

        private IEnumerator IPlayOneShot(Audios audio)
        {
            AudioSource audioSource;
            AudioClipData audioClipData = AudioManagerData.GetAudioClipData((int)audio);
            if (audioClipData == null)
                yield break;

                Metelab.Log(this, $"IPlayOneShot-first-audio:{audio}, ListAudioSource:{ListDeactiveAudioSource.Count}");
            if (ListDeactiveAudioSource.Count > 0)
            {
                audioSource = ListDeactiveAudioSource[0];
                Metelab.Log(this, $"IPlayOneShot-audioSource-clipName:{audioSource.clip.name}, name{audioSource.name}");
                ListDeactiveAudioSource.RemoveAt(0);
            }
            else
            {
                audioSource = CreateNewAudioSource();
            }

            ListActiveAudioSource.Add(audioSource);
            Metelab.Log(this, $"IPlayOneShot-AudioSource Name :{audioSource.name}");

            audioSource.volume = audioClipData.RelativeVolume;
            audioSource.loop = false;
            audioSource.clip = audioClipData.Clip;
           // audioSource.mute = !SettingsData.Instance.Sound;
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            if(ListActiveAudioSource.Contains(audioSource))
                ListActiveAudioSource.Remove(audioSource);

            if(!ListDeactiveAudioSource.Contains(audioSource))
                ListDeactiveAudioSource.Add(audioSource);

            Metelab.Log(this, $"IPlayOneShot-last-clipName:{audioSource.clip.name}, Dict:{DicLoopAudioSource.Count}, List:{ListDeactiveAudioSource.Count}, Active:{ListActiveAudioSource.Count}");
        }

        public void PlayLoop(Audios audio)
        {
            Metelab.Log(this, $"PlayLoop-first-Dict:{DicLoopAudioSource.Count}, List:{ListDeactiveAudioSource.Count}, Active:{ListActiveAudioSource.Count}");
            Metelab.Log(this, $"PlayLoop-audio:{audio}");
            AudioSource audioSource;
            AudioClipData audioClipData = AudioManagerData.GetAudioClipData((int)audio);
            if (audioClipData == null)
                return;

            if (DicLoopAudioSource.ContainsKey(audio))
            {
                Metelab.Log(this, "PlayLoop-audio is playing!!!");
                audioSource = DicLoopAudioSource[audio];
                if (audioSource.isPlaying)
                    return;
            }
            else
            {
                Metelab.Log(this, $"PlayLoop-audio:{audio}, ListAudioSource:{ListDeactiveAudioSource.Count}");
                if (ListDeactiveAudioSource.Count > 0)
                {
                    audioSource = ListDeactiveAudioSource[0];
                    Metelab.Log(this, $"PlayLoop-audioSource-clipName:{audioSource.clip.name}, name{audioSource.name}");
                    ListDeactiveAudioSource.RemoveAt(0);
                }
                else
                {
                    audioSource = CreateNewAudioSource();
                }
            }

            Metelab.Log(this, $"PlayLoop-AudioSource Name :{audioSource.name}");
            DicLoopAudioSource.Add(audio, audioSource);
            audioSource.volume = audioClipData.RelativeVolume;
            audioSource.loop = true;
            audioSource.clip = audioClipData.Clip;
           // audioSource.mute = !SettingsData.Instance.Music;
            audioSource.Play();
            Metelab.Log(this, $"PlayLoop-last-Dict:{DicLoopAudioSource.Count}, List:{ListDeactiveAudioSource.Count}, Active:{ListActiveAudioSource.Count}");
        }

        public void StopLoop(Audios audio)
        {
            Metelab.Log(this, $"StopLoop-audio:{audio}");
            if (DicLoopAudioSource.ContainsKey(audio))
            {
                AudioSource audioSource = DicLoopAudioSource[audio];
                audioSource.Stop();
                DicLoopAudioSource.Remove(audio);
                if (!ListDeactiveAudioSource.Contains(audioSource))
                    ListDeactiveAudioSource.Add(audioSource);
            }
        }

        private void StopAllLoop()
        {
            foreach (KeyValuePair<Audios, AudioSource> item in DicLoopAudioSource)
            {
                if (item.Value != null)
                {
                    item.Value.Stop();
                    if (!ListDeactiveAudioSource.Contains(item.Value))
                        ListDeactiveAudioSource.Add(item.Value);
                }
            }
            DicLoopAudioSource.Clear();
        }


        public void SlowStopLoop(Audios audio, float stopTimeSec)
        {
            Metelab.Log(this, $"SlowStopLoop-audio:{audio}");
            StartCoroutine(ISlowStopLoop(audio, stopTimeSec));
        }

        private IEnumerator ISlowStopLoop(Audios audio, float stopTimeSec)
        {
            if (DicLoopAudioSource.ContainsKey(audio))
            {
                AudioSource audioSource = DicLoopAudioSource[audio];
                float startVolume = audioSource.volume;
                float timer = 0;
                while (audioSource.volume > 0)
                {
                    audioSource.volume = startVolume*(1-(timer/ stopTimeSec));
                    timer += Time.deltaTime;
                    yield return null;
                }

                audioSource.Stop();
                DicLoopAudioSource.Remove(audio);

                if(!ListDeactiveAudioSource.Contains(audioSource))
                    ListDeactiveAudioSource.Add(audioSource);
            }
        }

        public void StopAll()
        {
            Metelab.Log(this, $"StopAll-before-Dict:{DicLoopAudioSource.Count}, List:{ListDeactiveAudioSource.Count}, Active:{ListActiveAudioSource.Count}");

            StopAllLoop();

            foreach (AudioSource item in ListActiveAudioSource)
            {
                if (item != null)
                {
                    item.Stop();
                    if (!ListDeactiveAudioSource.Contains(item))
                        ListDeactiveAudioSource.Add(item);
                }
            }
            ListActiveAudioSource.Clear();
            Metelab.Log(this, $"StopAll-after-Dict:{DicLoopAudioSource.Count}, List:{ListDeactiveAudioSource.Count}, Active:{ListActiveAudioSource.Count}");
        }

        private AudioSource CreateNewAudioSource()
        {
            AudioSource audioSource = new GameObject($"AS_{mAudioSourceCount}", typeof(AudioSource)).GetComponent<AudioSource>();
            audioSource.transform.parent = transform;
            mAudioSourceCount++;
            return audioSource;
        }

        private void ChangeBackgroundMusic(Audios audio)
        {
            Metelab.Log(this, $"audio:{audio}");
            StopAllLoop();
            PlayLoop(audio);
        }


        #region Events

        private void OnGlobalClick()
        {
            PlayOneShot(Audios.ButtonClick);
        }

        private void OnChangedGameMusic(Audios audio)
        {
            ChangeBackgroundMusic(audio);
        }

        #endregion
    }
}
