#pragma once
#include "PluginSettings.h"
#include <iostream>
#include <fstream>
#include <vector>


class PLUGIN_API CheckpointTimeLogger
{
public:
	void ResetLogger();

	void SaveCheckpointTime(float RTBC);

	float GetTotalTime();
	float GetCheckpointTime(int index);
	int GetNumCheckpoints();

private:
	float m_TRT = 0.0f;
	std::vector<float> m_RTBC;
};